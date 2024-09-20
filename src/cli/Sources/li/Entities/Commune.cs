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

using System;

namespace OpenPlzApi.CLI.Sources.LI
{
    /// <summary>
    /// Liechtenstein commune (Gemeinde)
    /// </summary>
    public class Commune : BaseRecord
    {
        /// <summary>
        /// Electoral district 
        /// </summary>
        public string ElectoralDistrict { get; set; }

        /// <summary>
        /// Date of last modification
        /// </summary>
        public DateOnly? LastModified { get; set; }

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
