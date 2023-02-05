﻿#region OpenPLZ API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
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

using Enbrea.Progress;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OpenPlzApi.DataLayer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI
{
    /// <summary>
    /// Manager for creating the database
    /// </summary>
    public class DbMigrator
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ProgressReport _progressReport;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMigrator"/> class.
        /// </summary>
        /// <param name="appConfiguration">Configuration data</param>
        public DbMigrator(AppConfiguration appConfiguration)
        {
            _dbContextFactory = new PooledDbContextFactory<AppDbContext>(AppDbContextOptionsFactory.CreateDbContextOptions(appConfiguration.Database));
            _progressReport = ProgressReportFactory.CreateProgressReport(ProgressUnit.Count);
        }

        /// <summary>
        /// Executes the database creation 
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = _dbContextFactory.CreateDbContext();

                _progressReport.Caption("Migration");

                _progressReport.Start("Delete existing database...");
                await dbContext.Database.EnsureDeletedAsync(cancellationToken);
                _progressReport.Finish();

                _progressReport.Start("Creating new database...");
                await dbContext.Database.MigrateAsync(cancellationToken);
                _progressReport.Finish();

                _progressReport.Success("Database newly created!");
                _progressReport.NewLine();
            }
            catch (Exception ex)
            {
                _progressReport.Cancel();
                _progressReport.NewLine();
                _progressReport.Error($"Migration failed. {ex.Message}");
                throw;
            }
        }
    }
}
