#region OpenPLZ API - Copyright (C) 2022 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2022 STÜBER SYSTEMS GmbH
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU Affero General Public License, version 3,
 *    as published by the Free Software Foundation.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *    GNU Affero General Public License for more details.
 *
 *    You should have received a copy of the GNU Affero General Public License
 *    along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion

using ICSharpCode.SharpZipLib.Zip;
using Microsoft.EntityFrameworkCore;
using OpenPlzApi.DataLayer;
using OpenPlzApi.DataLayer.DE;
using OpenPlzApi.CLI.Sources.DE;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI.DE
{
    public class MunicipalitiesImporter : BaseImporter
    {
        private readonly FileInfo _cachedSourceFile;
        private readonly FileInfo _cachedZipArchive;
        private readonly Uri _remoteZipArchive;

        public MunicipalitiesImporter(IDbContextFactory<AppDbContext> dbContextFactory, string caption, Uri remoteZipArchive, FileInfo cachedZipArchive, FileInfo cachedSourceFile)
            : base(dbContextFactory, caption)
        {
            _remoteZipArchive = remoteZipArchive;
            _cachedZipArchive = cachedZipArchive;
            _cachedSourceFile = cachedSourceFile;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await DownloadToCacheAsync(cancellationToken);
            await ImportToDatabaseAsync(cancellationToken);
        }

        private async Task DownloadToCacheAsync(CancellationToken cancellationToken)
        {
            if (!_cachedZipArchive.Exists)
            {
                _progressReport.Start($"Download {_cachedZipArchive.Name}");

                Directory.CreateDirectory(_cachedZipArchive.DirectoryName);

                await _httpClient.DownloadAsync(_remoteZipArchive, _cachedZipArchive, cancellationToken);

                _progressReport.Finish();
            }

            if (!_cachedSourceFile.Exists)
            {
                _progressReport.Start($"Extract {_cachedSourceFile.Name}");

                Directory.CreateDirectory(_cachedSourceFile.DirectoryName);

                var fastZip = new FastZip();
                fastZip.ExtractZip(_cachedZipArchive.FullName, _cachedSourceFile.DirectoryName, null);

                _progressReport.Finish();
            }
        }

        private async Task ImportToDatabaseAsync(CancellationToken cancellationToken)
        {
            uint recordCount = 0;
            uint federalStateCount = 0;
            uint governmentRegionCount = 0;
            uint districtCount = 0;
            uint municipalAssociationCount = 0;
            uint municipalityCount = 0;

            try
            {
                _progressReport.Start("Open GV100AD file");

                using var gvFileStream = _cachedSourceFile.OpenText();

                var gvReader = new GV100AD.GV100ADReader(gvFileStream);

                _progressReport.Finish();

                _progressReport.Start("Read and process records...");

                await foreach (var gvRecord in gvReader.ReadAsync())
                {
                    if (gvRecord is GV100AD.FederalState federalState)
                    {
                        using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                        dbContext.Set<FederalState>().Add(new FederalState()
                        {
                            Id = federalState.GetUniqueId(),
                            Name = federalState.Name,
                            RegionalKey = federalState.RegionalCode,
                            SeatOfGovernment =federalState.SeatOfGovernment,
                            Source = "GV100AD",
                            TimeStamp = federalState.TimeStamp
                        });

                        federalStateCount++;

                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                    else if (gvRecord is GV100AD.GovernmentRegion governmentRegion)
                    {
                        using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                        dbContext.Set<GovernmentRegion>().Add(new GovernmentRegion()
                        {
                            Id = governmentRegion.GetUniqueId(),
                            Name = governmentRegion.Name,
                            RegionalKey = governmentRegion.RegionalCode,
                            AdministrativeHeadquarters = governmentRegion.AdministrativeHeadquarters,
                            FederalStateId = governmentRegion.GetFederalStatenUniqueId(),
                            Source = "GV100AD",
                            TimeStamp = governmentRegion.TimeStamp
                        });

                        governmentRegionCount++;

                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                    else if (gvRecord is GV100AD.District district)
                    {
                        using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                        dbContext.Set<District>().Add(new District()
                        {
                            Id = district.GetUniqueId(),
                            Name = district.Name,
                            RegionalKey = district.RegionalCode,
                            Type = (DistrictType)district.Type,
                            AdministrativeHeadquarters = district.AdministrativeHeadquarters,
                            FederalStateId = district.GetFederalStatenUniqueId(),
                            GovernmentRegionId = district.GetGovernmentRegionUniqueId(),
                            Source = "GV100AD",
                            TimeStamp = district.TimeStamp
                        });

                        districtCount++;

                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                    else if (gvRecord is GV100AD.MunicipalAssociation association)
                    {
                        using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                        dbContext.Set<MunicipalAssociation>().Add(new MunicipalAssociation()
                        {
                            Id = association.GetUniqueId(),
                            Name = association.Name,
                            RegionalKey = association.RegionalCode,
                            Code = association.Association,
                            Type = (MunicipalAssociationType)association.Type,
                            AdministrativeHeadquarters = association.AdministrativeHeadquarters,
                            DistrictId = association.GetDistrictUniqueId(),
                            Source = "GV100AD",
                            TimeStamp = association.TimeStamp
                        });

                        municipalAssociationCount++;

                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                    else if (gvRecord is GV100AD.Municipality municipality)
                    {

                        using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                        dbContext.Set<Municipality>().Add(new Municipality()
                        {
                            Id = municipality.GetUniqueId(),
                            Name = municipality.Name,
                            ShortName = municipality.GetShortName(),
                            RegionalKey = municipality.RegionalCode,
                            Type = (MunicipalityType)municipality.Type,
                            DistrictId = municipality.GetDistrictUniqueId(),
                            AssociationId = municipality.GetMunicipalAssociationUniqueId(),
                            PostalCode = municipality.PostalCode,
                            MultiplePostalCodes = municipality.MultiplePostalCodes,
                            Source = "GV100AD",
                            TimeStamp = municipality.TimeStamp
                        });

                        municipalityCount++;

                        await dbContext.SaveChangesAsync(cancellationToken);
                    }

                    _progressReport.Continue(recordCount++);
                }

                _progressReport.Finish(recordCount);
                _progressReport.Success(
                    $"{federalStateCount} federal states, {governmentRegionCount} government regions, {districtCount} districts, " +
                    $"{municipalAssociationCount} municipal and {municipalityCount} municipalities imported.");
                _progressReport.NewLine();
            }
            catch (Exception ex)
            {
                _progressReport.Cancel();
                _progressReport.Error($"Import failed. {ex.Message}");
                throw;
            }
        }
    }
}
