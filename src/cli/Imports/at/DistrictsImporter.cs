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
using OpenPlzApi.DataLayer.AT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI.AT
{
    public class DistrictsImporter : BaseImporter
    {
        private readonly FileInfo _cachedSourceFile;
        private readonly Uri _remoteSourceFile;

        public DistrictsImporter(IDbContextFactory<AppDbContext> dbContextFactory, string caption, Uri remoteSourceFile, FileInfo cachedSourceFile)
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
            uint federalProvinceCount = 0;
            uint districtCount = 0;

            var federalProvinceIdCache = new HashSet<Guid>();

            try
            {
                _consoleWriter.StartProgress($"Open {_cachedSourceFile.Name} file");

                using var rdFileStream = _cachedSourceFile.OpenText();

                var rdReader = new Sources.AT.DistrictDataReader(rdFileStream);

                _consoleWriter.FinishProgress();

                _consoleWriter.StartProgress("Read and process districts...");

                await foreach (var district in rdReader.ReadAsync(cancellationToken))
                {
                    using var dbContext = _dbContextFactory.CreateDbContext();

                    if (!federalProvinceIdCache.Contains(district.FederalProvince.GetUniqueId()))
                    {
                        dbContext.Set<FederalProvince>().Add(new FederalProvince()
                        {
                            Id = district.FederalProvince.GetUniqueId(),
                            Key = district.FederalProvince.Key,
                            Name = district.FederalProvince.Name
                        });

                        federalProvinceIdCache.Add(district.FederalProvince.GetUniqueId());
                        federalProvinceCount++;
                    }

                    dbContext.Set<District>().Add(new District()
                    {
                        Id = district.GetUniqueId(),
                        Key = district.Key,
                        Name = district.Name.GetFriendlyName(),
                        Code = district.Code,
                        FederalProvinceId = district.FederalProvince.GetUniqueId()
                    });

                    await dbContext.SaveChangesAsync(cancellationToken);

                    districtCount++;

                    _consoleWriter.ContinueProgress(++recordCount);
                }

                _consoleWriter
                    .FinishProgress(recordCount)
                    .Success($"{federalProvinceCount} federal provinces and {districtCount} districts imported.")
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

