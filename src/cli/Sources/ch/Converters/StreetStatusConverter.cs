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
    public class StreetStatusConverter : ICsvConverter
    {
        public virtual object FromString(string value)
        {
            return value switch
            {
                "" or null => StreetStatus.None,
                "planned" => StreetStatus.Planned,
                "existing" => StreetStatus.Existing,
                "outdated" => StreetStatus.Outdated,
                _ => throw new NotSupportedException($"Street status {value} not supported"),
            };
        }

        public string ToString(object value)
        {
            return null;
        }
    }
}
