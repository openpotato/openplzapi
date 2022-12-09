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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OpenPlzApi.DataLayer;
using OpenPlzApi.DataLayer.AT;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI.AT
{
    public class MunicipalitiesImporter : BaseImporter
    {
        private readonly FileInfo _cachedSourceFile;
        private readonly Uri _remoteSourceFile;

        public MunicipalitiesImporter(IDbContextFactory<AppDbContext> dbContextFactory, string caption, Uri remoteSourceFile, FileInfo cachedSourceFile)
            : base(dbContextFactory, caption)
        {
            _remoteSourceFile = remoteSourceFile;
            _cachedSourceFile = cachedSourceFile;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await DownloadToCacheAsync(cancellationToken);
            await ImportToDatabaseAsync(cancellationToken);
        }

        private async Task DownloadToCacheAsync(CancellationToken cancellationToken)
        {
            if (!_cachedSourceFile.Exists)
            {
                _progressReport.Start($"Download {_cachedSourceFile.Name}");

                Directory.CreateDirectory(_cachedSourceFile.DirectoryName);

                await _httpClient.DownloadAsync(_remoteSourceFile, _cachedSourceFile, cancellationToken);

                _progressReport.Finish();
            }
        }

        private async Task ImportToDatabaseAsync(CancellationToken cancellationToken)
        {
            uint recordCount = 0;
            uint municipalitiesCount = 0;

            try
            {
                _progressReport.Start($"Open {_cachedSourceFile.Name} file");

                using var gvFileStream = _cachedSourceFile.OpenText();

                var rdReader = new Sources.AT.MunicipalityDataReader(gvFileStream);

                _progressReport.Finish();

                _progressReport.Start("Read and process municipalities...");

                await foreach (var municipality in rdReader.ReadAsync(cancellationToken))
                {
                    using var dbContext = _dbContextFactory.CreateDbContext();

                    dbContext.Set<Municipality>().Add(new Municipality()
                    {
                        Id = municipality.GetUniqueId(),
                        Key = municipality.Key,
                        Name = municipality.Name,
                        Code = municipality.Code,
                        Status = (MunicipalityStatus)municipality.Status,
                        PostalCode = municipality.PostalCode,
                        MultiplePostalCodes = municipality.AdditionalPostalCodes.Length > 0,
                        DistrictId = municipality.District.GetUniqueId(),
                        Source = "statistik.at",
                        TimeStamp = municipality.TimeStamp
                    });

                    await dbContext.SaveChangesAsync(cancellationToken);

                    municipalitiesCount++;
                    
                    _progressReport.Continue(recordCount++);
                }

                _progressReport.Finish(recordCount);
                _progressReport.Success($"{municipalitiesCount} municipalities imported.");
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