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

namespace OpenPlzApi.CLI.Sources.AT
{
    public class StringArrayConverter : ICsvConverter
    {
        public virtual object FromString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Array.Empty<string>();
            }
            else 
            {
                var csvParser = new CsvLineParser(separator: ' ');
                return csvParser.Parse(value);
            }
        }

        public string ToString(object value) => null;
    }
}
