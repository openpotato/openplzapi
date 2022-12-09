#region OpenPLZ API - Copyright (C) 2022 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2022 STÜBER SYSTEMS GmbH
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

namespace OpenPlzApi.CLI.Sources.AT
{
    /// <summary>
    /// A reader for the Austrian official street list
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
            _csvReader.SetFormats<DateTime>("dd.MM.yyyy HH:mm:ss");
        }

        /// <summary>
        /// Iterates over the internal CSV stream and gives back street records
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An async enumerator of <see cref="Street"/> instances</returns>
        public async IAsyncEnumerable<Street> ReadAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await _csvReader.SkipAsync(1);
            await _csvReader.ReadAsync();

            var timeStamp = DateOnly.FromDateTime(_csvReader.GetValue<DateTime>(1));

            await _csvReader.ReadHeadersAsync();

            var municipalityCache = new Dictionary<string, Street._Locality._Municipality>();
            var localityCache = new Dictionary<string, Street._Locality>();

            while (await _csvReader.ReadAsync() > 1)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return GetStreet(timeStamp, municipalityCache, localityCache);
            }
        }

        private Street GetStreet(DateOnly timeStamp, Dictionary<string, Street._Locality._Municipality> municipalityCache, Dictionary<string, Street._Locality> localityCache)
        {
            var municipalityId = _csvReader.GetValue<string>("Gemeindecode");

            if (!municipalityCache.TryGetValue(municipalityId, out var municipality))
            {
                municipality = new Street._Locality._Municipality()
                {
                    Key = _csvReader.GetValue<string>("Gemeindekennziffer"),
                    Name = _csvReader.GetValue<string>("Politische Gemeinde"),
                    Code = _csvReader.GetValue<string>("Gemeindecode"),
                };

                municipalityCache.Add(municipalityId, municipality);
            }

            var localityId = $"{_csvReader.GetValue<string>("Ortschaftskennziffer")}+{_csvReader.GetValue<string>("Postleitzahl")}+{municipality.GetUniqueId()}";

            if (!localityCache.TryGetValue(localityId, out var locality))
            {
                locality = new Street._Locality()
                {
                    Key = _csvReader.GetValue<string>("Ortschaftskennziffer"),
                    PostalCode = _csvReader.GetValue<string>("Postleitzahl"),
                    Name = _csvReader.GetValue<string>("Ortschaftsname"),
                    Municipality = municipality
                };

                localityCache.Add(localityId, locality);
            }

            return new Street(timeStamp)
            {
                Key = _csvReader.GetValue<string>("Straßenkennziffer"),
                Name = _csvReader.GetValue<string>("Straßenname"),
                Locality = locality,
            };
        }
    }
}
