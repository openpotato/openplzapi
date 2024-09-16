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

namespace OpenPlzApi.CLI.Sources.CH
{
    /// <summary>
    /// A reader for the Swiss official street directory
    /// </summary>
    public class StreetDirectoryReader
    {
        private readonly CsvTableReader _csvReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreetDirectoryReader"/> class.
        /// </summary>
        /// <param name="textReader">The text reader to be read from.</param>
        public StreetDirectoryReader(TextReader textReader)
        {
            _csvReader = new CsvTableReader(textReader);
            _csvReader.Configuration.Separator = ';';
            _csvReader.AddConverter<StreetStatus>(new StreetStatusConverter());
            _csvReader.AddConverter<StreetType>(new StreetTypeConverter());
            _csvReader.AddConverter<List<string>>(new StringListConverter());
            _csvReader.SetFormats<DateOnly>("dd.MM.yyyy");
            _csvReader.SetTrueFalseString<bool>("true", "false");
        }

        /// <summary>
        /// Iterates over the internal CSV stream and gives back street records
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An async enumerator of <see cref="Street"/> instances</returns>
        public async IAsyncEnumerable<Street> ReadAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await _csvReader.ReadHeadersAsync();

            var communeCache = new Dictionary<string, Street._Locality._Commune>();
            var localityCache = new Dictionary<string, Street._Locality>();

            while (await _csvReader.ReadAsync() > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return GetStreet(communeCache, localityCache);
            }
        }

        private Street GetStreet(Dictionary<string, Street._Locality._Commune> communeCache, Dictionary<string, Street._Locality> localityCache)
        {
            var communeId = _csvReader.GetValue<string>("COM_FOSNR");

            if (!communeCache.TryGetValue(communeId, out var commune))
            {
                commune = new Street._Locality._Commune()
                {
                    Key = _csvReader.GetValue<string>("COM_FOSNR"),
                    Name = _csvReader.GetValue<string>("COM_NAME"),
                    Canton = _csvReader.GetValue<string>("COM_CANTON"),
                };

                communeCache.Add(communeId, commune);
            }

            var street = new Street()
            {
                Key = _csvReader.GetValue<string>("STR_ESID"),
                Name = _csvReader.GetValue<string>("STN_LABEL"),
                Type = _csvReader.GetValue<StreetType>("STR_TYPE"),
                Status = _csvReader.GetValue<StreetStatus>("STR_STATUS"),
                Official = _csvReader.GetValue<bool>("STR_OFFICIAL"),
                LastModified = _csvReader.GetValue<DateOnly>("STR_MODIFIED"),
            };

            var zipLabelList = _csvReader.GetValue<List<string>>("ZIP_LABEL");

            foreach (var zipLabel in zipLabelList)
            {
                if (!localityCache.TryGetValue(zipLabel, out var locality))
                {
                    locality = new Street._Locality()
                    {
                        PostalCode = zipLabel.Substring(0, 4),
                        Name = zipLabel.Substring(5),
                        Commune = commune
                    };

                    localityCache.Add(zipLabel, locality);
                }
                street.Localities.Add(locality);
            }

            return street;
        }
    }
}
