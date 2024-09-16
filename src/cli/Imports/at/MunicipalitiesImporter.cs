#region OpenPLZ API - Copyright (c) STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (c) STÜBER SYSTEMS GmbH
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
using OpenPlzApi.DataLayer;
using OpenPlzApi.DataLayer.AT;
using System;
using System.IO;
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
                _consoleWriter.StartProgress($"Download {_cachedSourceFile.Name}");

                Directory.CreateDirectory(_cachedSourceFile.DirectoryName);

                await _httpClient.DownloadAsync(_remoteSourceFile, _cachedSourceFile, cancellationToken);

                _consoleWriter.FinishProgress();
            }
        }

        private async Task ImportToDatabaseAsync(CancellationToken cancellationToken)
        {
            uint recordCount = 0;
            uint municipalitiesCount = 0;

            try
            {
                _consoleWriter.StartProgress($"Open {_cachedSourceFile.Name} file");

                using var gvFileStream = _cachedSourceFile.OpenText();

                var rdReader = new Sources.AT.MunicipalityDataReader(gvFileStream);

                _consoleWriter.FinishProgress();

                _consoleWriter.StartProgress("Read and process municipalities...");

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
                        DistrictId = municipality.District.GetUniqueId()
                    });

                    await dbContext.SaveChangesAsync(cancellationToken);

                    municipalitiesCount++;
                    
                    _consoleWriter.ContinueProgress(++recordCount);
                }

                _consoleWriter
                    .FinishProgress(recordCount)
                    .Success($"{municipalitiesCount} municipalities imported.")
                    .NewLine();
            }
            catch (Exception ex)
            {
                _consoleWriter
                    .CancelProgress()
                    .Error($"Import failed. {ex.Message}");

                throw;
            }
        }
    }
}