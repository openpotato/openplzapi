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

using Swashbuckle.AspNetCore.Annotations;

namespace OpenPlzApi.CH
{
    /// <summary>
    /// Street status (Straßenstatus)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public enum StreetStatus
    {
        /// <summary>
        /// No status available
        /// </summary>
        None,
        
        /// <summary>
        /// Planned street
        /// </summary>
        Planned = 1,

        /// <summary>
        /// Real street
        /// </summary>
        Real = 2,

        /// <summary>
        /// Outdated street
        /// </summary>
        Outdated = 3
    }
}
