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

using OpenPlzApi.GV100AD;
using System;

namespace OpenPlzApi.CLI.Sources.DE
{
    public static class MunicipalityExtensions
    {
        public static Guid? GetMunicipalAssociationUniqueId(this Municipality record)
        {
            if (record.Association != "0000")
            {
                return IdFactory.CreateIdFromValue($"{record.RegionalCode.Substring(0, 5)}+{record.Association}");
            }
            else 
            {
                return default;
            }
        }
    }
}