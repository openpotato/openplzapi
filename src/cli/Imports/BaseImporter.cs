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

using Enbrea.Progress;
using Microsoft.EntityFrameworkCore;
using OpenPlzApi.DataLayer;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI
{
    public abstract class BaseImporter
    {
        protected readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        protected readonly IDownloadHttpClient _httpClient;
        protected readonly ProgressReport _progressReport;

        public BaseImporter(IDbContextFactory<AppDbContext> dbContextFactory, string caption)
        {
            _dbContextFactory = dbContextFactory;
            _httpClient = DownloadHttpClientFactory.CreateClient();
            _progressReport = ProgressReportFactory.CreateProgressReport(ProgressUnit.Count);
            _progressReport.Caption(caption);
        }

        public abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
