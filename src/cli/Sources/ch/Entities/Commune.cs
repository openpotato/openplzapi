#region OpenPLZ API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
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

using DocumentFormat.OpenXml.EMMA;
using System;

namespace OpenPlzApi.CLI.Sources.CH
{
    /// <summary>
    /// Swiss commune (Gemeinde)
    /// </summary>
    public class Commune : BaseRecord
    {
        /// <summary>
        /// Refrence to canton
        /// </summary>
        public Canton Canton { get; set; }

        /// <summary>
        /// Refrence to district
        /// </summary>
        public District District { get; set; }

        /// <summary>
        /// Date of last modification
        /// </summary>
        public DateOnly? LastModified { get; set; }

        /// <summary>
        /// Short name 
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Get a predictable unique id for this commune
        /// </summary>
        /// <returns>A guid value</returns>
        public Guid GetUniqueId()
        {
            return IdFactory.CreateIdFromValue(Key);
        }
    }
}
