#region OpenPLZ API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI.AT
{
    public class StreetsImporter : BaseImporter
    {
        private readonly FileInfo _cachedSourceFile;
        private readonly Uri _remoteSourceFile;

        public StreetsImporter(IDbContextFactory<AppDbContext> dbContextFactory, string caption, Uri remoteSourceFile, FileInfo cachedSourceFile)
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
            uint localityCount = 0;
            uint streetCount = 0;

            var localityIdCache = new HashSet<Guid>();

            try
            {
                _progressReport.Start($"Open {_cachedSourceFile.Name} file");

                using var rdFileStream = _cachedSourceFile.OpenText();

                var rdReader = new Sources.AT.StreetDataReader(rdFileStream);

                _progressReport.Finish();

                _progressReport.Start("Read and process streets...");

                await foreach (var street in rdReader.ReadAsync(cancellationToken))
                {
                    using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                    if (!localityIdCache.Contains(street.Locality.GetUniqueId()))
                    {
                        dbContext.Set<Locality>().Add(new Locality()
                        {
                            Id = street.Locality.GetUniqueId(),
                            Key = street.Locality.Key,
                            Name = street.Locality.Name,
                            PostalCode = street.Locality.PostalCode,
                            MunicipalityId = street.Locality.Municipality.GetUniqueId(),
                            Source = "statistik.at",
                            TimeStamp = street.TimeStamp
                        });

                        localityIdCache.Add(street.Locality.GetUniqueId());
                        localityCount++;
                    }

                    dbContext.Set<Street>().Add(new Street()
                    {
                        Id = Guid.NewGuid(),
                        Key = street.Key,
                        Name = street.Name,
                        LocalityId = street.Locality.GetUniqueId(),
                        Source =  "statistik.at",
                        TimeStamp = street.TimeStamp
                    });

                    await dbContext.SaveChangesAsync(cancellationToken);

                    streetCount++;

                    _progressReport.Continue(recordCount++);
                }

                _progressReport.Finish(recordCount);
                _progressReport.Success($"{localityCount} localities and {streetCount} streets imported.");
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