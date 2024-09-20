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

using Enbrea.Csv;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace OpenPlzApi.CLI.Sources.LI
{
    /// <summary>
    /// A reader for the Liechtenstein commune list
    /// </summary>
    public class CommuneDataReader
    {
        private readonly CsvTableReader _csvReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommuneDataReader"/> class for the specified stream.
        /// </summary>
        /// <param name="textReader">A text reader</param>
        public CommuneDataReader(TextReader textReader)
        {
            _csvReader = new CsvTableReader(textReader);
            _csvReader.Configuration.Separator = ',';
        }

        /// <summary>
        /// Iterates over the internal CSV stream and gives back commune records
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An async enumerator of <see cref="Commune"/> instances</returns>
        public async IAsyncEnumerable<Commune> ReadAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await _csvReader.ReadHeadersAsync();

            while (await _csvReader.ReadAsync() > 1)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return GetCommune();
            }
        }


        private Commune GetCommune()
        {
            return new Commune()
            {
                Key = _csvReader.GetValue<string>("Key"),
                Name = _csvReader.GetValue<string>("Name"),
                ElectoralDistrict = _csvReader.GetValue<string>("ElectoralDistrict"),
            };
        }
    }
}
