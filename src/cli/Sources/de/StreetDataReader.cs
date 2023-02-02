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

using Enbrea.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace OpenPlzApi.CLI.Sources.DE
{
    /// <summary>
    /// A reader for German street names from the OpenStreetMap project.
    /// </summary>
    public class StreetDataReader
    {
        private readonly CsvTableReader _csvReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreetDataReader"/> class for the specified stream.
        /// </summary>
        /// <param name="textReader">A text reader</param>
        public StreetDataReader(TextReader textReader)
        {
            _csvReader = new CsvTableReader(textReader);
            _csvReader.Configuration.Separator = ';';
        }

        /// <summary>
        /// Iterates over the internal CSV stream and gives back street records
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An async enumerator of <see cref="Street"/> instances</returns>
        public async IAsyncEnumerable<Street> ReadAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await _csvReader.ReadHeadersAsync();

            var localityCache = new Dictionary<string, Street._Locality>();

            while (await _csvReader.ReadAsync() > 1)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return GetStreet(localityCache);
            }
        }

        private Street GetStreet(Dictionary<string, Street._Locality> localityCache)
        {
            var localityId = $"{_csvReader.GetValue<string>("Name")}+{_csvReader.GetValue<string>("PostalCode")}+{_csvReader.GetValue<string>("RegionalKey")}";

            if (!localityCache.TryGetValue(localityId, out var locality))
            {
                locality = new Street._Locality()
                {
                    PostalCode = _csvReader.GetValue<string>("PostalCode"),
                    Name = _csvReader.GetValue<string>("Locality"),
                    MunicipalityKey = _csvReader.GetValue<string>("RegionalKey")
                };

                localityCache.Add(localityId, locality);
            }

            return new Street()
            {
                Name = _csvReader.GetValue<string>("Name"),
                Locality = locality,
            };
        }
    }
}
