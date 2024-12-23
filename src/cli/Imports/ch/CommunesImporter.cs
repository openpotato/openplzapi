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
using OpenPlzApi.AGVCH;
using OpenPlzApi.CLI.Sources.CH;
using OpenPlzApi.DataLayer;
using OpenPlzApi.DataLayer.CH;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI.CH
{
    public class CommunesImporter : BaseImporter
    {
        private readonly FileInfo _cachedSourceFile;

        public CommunesImporter(IDbContextFactory<AppDbContext> dbContextFactory, string caption, FileInfo cachedSourceFile)
            : base(dbContextFactory, caption)
        {
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

                var apiClient = AGVCHApiClientFactory.CreateClient();

                await apiClient.DownloadSnapshotDocumentAsync(_cachedSourceFile, DateOnly.FromDateTime(DateTime.Now), cancellationToken);

                _consoleWriter.FinishProgress();
            }
        }

        private async Task ImportToDatabaseAsync(CancellationToken cancellationToken)
        {
            uint recordCount = 0;
            uint cantonsCount = 0;
            uint districtsCount = 0;
            uint communesCount = 0;

            try
            {
                _consoleWriter.StartProgress($"Open {_cachedSourceFile.Name} file");

                using var gvFileStream = _cachedSourceFile.OpenRead();

                var communeRegister = new Sources.CH.CommuneRegister();

                await communeRegister.Load(gvFileStream);

                _consoleWriter.FinishProgress();
                
                _consoleWriter.StartProgress("Read and process cantons...");

                foreach (var canton in communeRegister.Cantons)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                    dbContext.Set<DataLayer.CH.Canton>().Add(new DataLayer.CH.Canton()
                    {
                        Id = canton.GetUniqueId(),
                        Key = canton.Key,
                        HistoricalCode = canton.HistoricalCode,
                        Name = canton.Name,
                        ShortName = canton.ShortName
                    });

                    await dbContext.SaveChangesAsync(cancellationToken);

                    cantonsCount++;

                    _consoleWriter.ContinueProgress(++recordCount);
                }

                _consoleWriter.FinishProgress(recordCount);

                recordCount = 0;

                _consoleWriter.StartProgress("Read and process districts...");

                foreach (var district in communeRegister.Districts)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                    dbContext.Set<DataLayer.CH.District>().Add(new DataLayer.CH.District()
                    {
                        Id = district.GetUniqueId(),
                        Key = district.Key,
                        HistoricalCode = district.HistoricalCode,
                        Name = district.Name,
                        ShortName = district.ShortName,
                        CantonId = district.Canton.GetUniqueId()
                    });

                    await dbContext.SaveChangesAsync(cancellationToken);

                    districtsCount++;

                    _consoleWriter.ContinueProgress(++recordCount);
                }

                _consoleWriter.FinishProgress(recordCount);

                recordCount = 0;

                _consoleWriter.StartProgress("Read and process communes...");

                foreach (var commune in communeRegister.Communes)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                    dbContext.Set<DataLayer.CH.Commune>().Add(new DataLayer.CH.Commune()
                    {
                        Id = commune.GetUniqueId(),
                        Key = commune.Key,
                        HistoricalCode = commune.HistoricalCode,
                        Name = commune.Name,
                        ShortName = commune.ShortName,
                        DistrictId = commune.District.GetUniqueId()
                    });


                    await dbContext.SaveChangesAsync(cancellationToken);

                    communesCount++;

                    _consoleWriter.ContinueProgress(++recordCount);
                }

                _consoleWriter
                    .FinishProgress(recordCount)
                    .Success($"{cantonsCount} cantons, {districtsCount} districts and {communesCount} communes imported.")
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

