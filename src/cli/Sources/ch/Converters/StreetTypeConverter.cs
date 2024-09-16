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

namespace OpenPlzApi.CLI.Sources.CH
{
    public class StreetTypeConverter : ICsvConverter
    {
        public virtual object FromString(string value)
        {
            return value switch
            {
                "" or null => StreetType.None,
                "Area" => StreetType.Area,
                "Street" => StreetType.Street,
                "Place" => StreetType.Place,
                _ => throw new NotSupportedException($"Street type {value} not supported"),
            };
        }

        public string ToString(object value)
        {
            return null;
        }
    }
}
