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
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace OpenPlzApi.CLI.Sources.AT
{
    /// <summary>
    /// A reader for the Austrian official municipality list
    /// </summary>
    public class MunicipalityDataReader
    {
        private readonly CsvTableReader _csvReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="MunicipalityDataReader"/> class for the specified stream.
        /// </summary>
        /// <param name="textReader">A text reader</param>
        public MunicipalityDataReader(TextReader textReader)
        {
            _csvReader = new CsvTableReader(textReader);
            _csvReader.Configuration.Separator = ';';
            _csvReader.AddConverter<MunicipalityStatus>(new MunicipalityStatusConverter());
            _csvReader.AddConverter<string[]>(new StringArrayConverter());
            _csvReader.SetFormats<DateTime>("dd.MM.yyyy HH:mm:ss");
        }

        /// <summary>
        /// Iterates over the internal CSV stream and gives back municipality records
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An async enumerator of <see cref="Municipality"/> instances</returns>
        public async IAsyncEnumerable<Municipality> ReadAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await _csvReader.SkipAsync(1);
            await _csvReader.ReadAsync();

            var timeStamp = DateOnly.FromDateTime(_csvReader.GetValue<DateTime>(1));

            await _csvReader.ReadHeadersAsync();

            var districtCache = new Dictionary<string, Municipality._District>();

            while (await _csvReader.ReadAsync() > 1)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return GenerateMunicipality(timeStamp, districtCache);
            }
        }

        private Municipality GenerateMunicipality(DateOnly timeStamp, Dictionary<string, Municipality._District> districtCache)
        {
            var districtId = _csvReader.GetValue<string>("Gemeindecode").Substring(0, 3);

            if (!districtCache.TryGetValue(districtId, out var district))
            {
                district = new Municipality._District()
                {
                    Code = _csvReader.GetValue<string>("Gemeindecode").Substring(0, 3),
                };

                districtCache.Add(districtId, district);
            }

            return new Municipality(timeStamp)
            {
                Key = _csvReader.GetValue<string>("Gemeindekennziffer"),
                Name = _csvReader.GetValue<string>("Gemeindename"),
                Code = _csvReader.GetValue<string>("Gemeindecode"),
                Status = _csvReader.GetValue<MunicipalityStatus>("Status"),
                PostalCode = _csvReader.GetValue<string>("PLZ des Gem.Amtes"),
                AdditionalPostalCodes = _csvReader.GetValue<string[]>("weitere Postleitzahlen"),
                District = district
            };
        }
    }
}
