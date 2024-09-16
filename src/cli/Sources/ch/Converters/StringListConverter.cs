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

namespace OpenPlzApi.CLI.Sources.CH
{
    public class StringListConverter : ICsvConverter
    {
        public virtual object FromString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new List<string>();
            }
            else
            {
                var list = new List<string>();
                var csvLineParser = new CsvLineParser(new CsvConfiguration { Separator = ',' });
                csvLineParser.Parse(value, (i, s) => { if (!string.IsNullOrEmpty(s)) list.Add(s.Trim()); });
                return list;
            }
        }

        public string ToString(object value)
        {
            return null;
        }
    }
}
