﻿#region OpenPLZ API - Copyright (c) STÜBER SYSTEMS GmbH
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
using OpenPlzApi.DataLayer.DE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI.DE
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
                _consoleWriter.StartProgress($"Download {_cachedSourceFile.Name}");

                Directory.CreateDirectory(_cachedSourceFile.DirectoryName);

                await _httpClient.DownloadAsync(_remoteSourceFile, _cachedSourceFile, cancellationToken);

                _consoleWriter.FinishProgress();
            }
        }

        private async Task ImportToDatabaseAsync(CancellationToken cancellationToken)
        {
            uint recordCount = 0;
            uint streetCount = 0;
            uint localityCount = 0;

            var localityIdCache = new HashSet<Guid>();

            try
            {
                _consoleWriter.StartProgress($"Open {_cachedSourceFile.Name} file");

                using var rdFileStream = _cachedSourceFile.OpenText();

                var rdReader = new Sources.DE.StreetDataReader(rdFileStream);

                _consoleWriter.FinishProgress();

                _consoleWriter.StartProgress("Read and process records...");

                await foreach(var street in rdReader.ReadAsync(cancellationToken))
                {
                    using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                    if (!localityIdCache.Contains(street.Locality.GetUniqueId()))
                    {
                        dbContext.Set<Locality>().Add(new Locality()
                        {
                            Id = street.Locality.GetUniqueId(),
                            PostalCode = street.Locality.PostalCode,
                            Name = street.Locality.Name,
                            MunicipalityId = street.Locality.GetUniqueMunicipalityId()
                        });

                        localityIdCache.Add(street.Locality.GetUniqueId());
                        localityCount++;
                    }

                    var streetId = Guid.NewGuid();

                    dbContext.Set<Street>().Add(new Street()
                    {
                        Id = streetId,
                        Name = street.Name,
                        LocalityId = street.Locality.GetUniqueId(),
                        Borough = string.IsNullOrEmpty(street.Borough) ? null : street.Borough,
                        Suburb = string.IsNullOrEmpty(street.Suburb) ? null : street.Suburb
                    });

                    dbContext.Set<FullTextStreet>().Add(new FullTextStreet()
                    {
                        Id = streetId,
                        Name = street.Name,
                        Locality = street.Locality.Name,
                        PostalCode = street.Locality.PostalCode,
                        MunicipalityId = street.Locality.GetUniqueMunicipalityId(),
                        Borough = string.IsNullOrEmpty(street.Borough) ? null : street.Borough,
                        Suburb = string.IsNullOrEmpty(street.Suburb) ? null : street.Suburb
                    });

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
