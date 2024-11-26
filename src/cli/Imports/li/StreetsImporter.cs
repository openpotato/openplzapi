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

using ICSharpCode.SharpZipLib.Zip;
using Microsoft.EntityFrameworkCore;
using OpenPlzApi.DataLayer;
using OpenPlzApi.DataLayer.LI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI.LI
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
                _consoleWriter.StartProgress($"Download {_cachedZipArchive.Name}");

                Directory.CreateDirectory(_cachedZipArchive.DirectoryName);

                await _httpClient.DownloadAsync(_remoteZipArchive, _cachedZipArchive, cancellationToken);

                _consoleWriter.FinishProgress();
            }

            if (!_cachedSourceFile.Exists)
            {

                _consoleWriter.StartProgress($"Extract {_cachedSourceFile.Name}");

                Directory.CreateDirectory(_cachedSourceFile.DirectoryName);

                var fastZip = new FastZip();
                fastZip.ExtractZip(_cachedZipArchive.FullName, _cachedSourceFile.DirectoryName, null);

                _consoleWriter.FinishProgress();
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
                _consoleWriter.StartProgress($"Open {_cachedSourceFile.Name} file");

                using var rdFileReader = _cachedSourceFile.OpenText();

                var rdReader = new Sources.LI.StreetDirectoryReader(rdFileReader);

                _consoleWriter.FinishProgress();

                _consoleWriter.StartProgress("Read and process streets...");

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
                                CommuneId = locality.Commune.GetUniqueId()
                            });

                            localityIdCache.Add(locality.GetUniqueId());
                            localityCount++;
                        }

                        var streetId = Guid.NewGuid();

                        dbContext.Set<Street>().Add(new Street()
                        {
                            Id = streetId,
                            Key = street.Key,
                            Name = street.Name,
                            LocalityId = locality.GetUniqueId(),
                            Status = (StreetStatus)street.Status
                        });

                        dbContext.Set<FullTextStreet>().Add(new FullTextStreet()
                        {
                            Id = streetId,
                            Key = street.Key,
                            Name = street.Name,
                            Locality = locality.Name,
                            PostalCode = locality.PostalCode,
                            CommuneId = locality.Commune.GetUniqueId(),
                            Status = (StreetStatus)street.Status
                        });
                    }

                    await dbContext.SaveChangesAsync(cancellationToken);
        
                    streetCount++;
                    
                    _consoleWriter.ContinueProgress(++recordCount);
                }

                _consoleWriter
                    .FinishProgress(recordCount)
                    .Success($"{localityCount} localities and {streetCount} streets imported.")
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
