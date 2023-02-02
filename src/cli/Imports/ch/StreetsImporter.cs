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

using ICSharpCode.SharpZipLib.Zip;
using Microsoft.EntityFrameworkCore;
using OpenPlzApi.DataLayer;
using OpenPlzApi.DataLayer.CH;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI.CH
{
    public class StreetsImporter : BaseImporter
    {
        private readonly FileInfo _cachedSourceFile;
        private readonly FileInfo _cachedZipArchive;
        private readonly Uri _remoteZipArchive;

        public StreetsImporter(IDbContextFactory<AppDbContext> dbContextFactory, string caption, Uri remoteZipArchive, FileInfo cachedZipArchive, FileInfo cachedSourceFile)
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
            uint localityCount = 0;
            uint streetCount = 0;

            var localityIdCache = new HashSet<Guid>();

            try
            {
                _progressReport.Start($"Open {_cachedSourceFile.Name} file");

                using var rdFileReader = _cachedSourceFile.OpenText();

                var rdReader = new Sources.CH.StreetDirectoryReader(rdFileReader);

                _progressReport.Finish();

                _progressReport.Start("Read and process streets...");

                await foreach (var street in rdReader.ReadAsync(cancellationToken))
                {
                    using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                    foreach (var locality in street.Localities)
                    {
                        if (!localityIdCache.Contains(locality.GetUniqueId()))
                        {
                            dbContext.Set<Locality>().Add(new Locality()
                            {
                                Id = locality.GetUniqueId(),
                                Name = locality.Name,
                                PostalCode = locality.PostalCode,
                                CommuneId = locality.Commune.GetUniqueId(),
                                Source = "cadastre.ch",
                                TimeStamp = street.LastModified
                            });

                            localityIdCache.Add(locality.GetUniqueId());
                            localityCount++;
                        }

                        dbContext.Set<Street>().Add(new Street()
                        {
                            Id = Guid.NewGuid(),
                            Key = street.Key,
                            Name = street.Name,
                            LocalityId = locality.GetUniqueId(),
                            Status = (StreetStatus)street.Status,
                            Source = "cadastre.ch",
                            TimeStamp = street.LastModified
                        });
                    }

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
