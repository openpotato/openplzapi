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
    public static class BaseRecordExtensions
    {
        public static Guid GetUniqueId(this BaseRecord record)
        {
            return IdFactory.CreateIdFromValue(record.RegionalCode);
        }

        public static Guid GetFederalStatenUniqueId(this BaseRecord record)
        {
            return IdFactory.CreateIdFromValue(record.RegionalCode[..2]);
        }

        public static Guid? GetGovernmentRegionUniqueId(this BaseRecord record)
        {
            if (record.RegionalCode.Substring(2, 1) != "0")
            {
                return IdFactory.CreateIdFromValue(record.RegionalCode[..3]);
            }
            else 
            {
                return default;
            }
        }

        public static Guid? GetDistrictUniqueId(this BaseRecord record)
        {
            if (record.RegionalCode.Substring(3, 2) != "00")
            {
                return IdFactory.CreateIdFromValue(record.RegionalCode[..5]);
            }
            else
            {
                return default;
            }
        }
    }
}