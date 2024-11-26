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
    /// A reader for the Austrian official district list
    /// </summary>
    public class DistrictDataReader
    {
        private readonly CsvTableReader _csvReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictDataReader"/> class.
        /// </summary>
        /// <param name="textReader">A text reader</param>
        public DistrictDataReader(TextReader textReader)
        {
            _csvReader = new CsvTableReader(textReader);
            _csvReader.Configuration.Separator = ';';
            _csvReader.SetFormats<DateTime>("dd.MM.yyyy HH:mm:ss");
        }

        /// <summary>
        /// Iterates over the internal CSV stream and gives back district records
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An async enumerator of <see cref="District"/> based instances</returns>
        public async IAsyncEnumerable<District> ReadAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await _csvReader.SkipAsync(1);
            await _csvReader.ReadAsync();

            var timeStamp = DateOnly.FromDateTime(_csvReader.GetValue<DateTime>(1));

            await _csvReader.ReadHeadersAsync();

            var federalProvinceCache = new Dictionary<string, District._FederalProvince>();

            while (await _csvReader.ReadAsync() > 1)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return GenerateDistrict(timeStamp, federalProvinceCache);
            }
        }

        private District GenerateDistrict(DateOnly timeStamp, Dictionary<string, District._FederalProvince> federalProvinceCache)
        {
            var federalProvinceId = _csvReader.GetValue<string>("Bundeslandkennziffer");

            if (!federalProvinceCache.TryGetValue(federalProvinceId, out var federalProvince))
            {
                federalProvince = new District._FederalProvince()
                {
                    Key = _csvReader.GetValue<string>("Bundeslandkennziffer"),
                    Name = _csvReader.GetValue<string>("Bundesland"),
                };

                federalProvinceCache.Add(federalProvinceId, federalProvince);
            }

            return new District(timeStamp)
            {
                Key = _csvReader.GetValue<string>("Kennziffer pol. Bezirk"),
                Name = _csvReader.GetValue<string>("Politischer Bezirk"),
                Code = _csvReader.GetValue<string>("Politischer Bez. Code"),
                FederalProvince = federalProvince
            };
        }
    }
}
