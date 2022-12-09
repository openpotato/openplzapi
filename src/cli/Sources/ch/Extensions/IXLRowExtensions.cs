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

using ClosedXML.Excel;

namespace OpenPlzApi.CLI.Sources.CH
{
    /// <summary>
    /// Extensions for <see cref="IXLRow"/>
    /// </summary>
    public static class IXLRowExtensions
    {
        public static T GetCellValue<T>(this IXLRow xlsRow, string xlsColumnName)
        {
            if (xlsColumnName != null)
            {
                return xlsRow.Cell(xlsColumnName).GetValue<T>();
            }
            else
            {
                return default;
            }
        }
    }
}
