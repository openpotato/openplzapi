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

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI
{
    public class DownloadHttpClient : IDownloadHttpClient
    {
        private readonly HttpClient _client;

        public DownloadHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task DownloadAsync(Uri requestUri, FileInfo targetFile, CancellationToken cancellationToken)
        {
            using var file = await _client.GetStreamAsync(requestUri).ConfigureAwait(false);

            using var fileStream = new FileStream(targetFile.FullName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

            await file.CopyToAsync(fileStream, cancellationToken);
        }
    }
}