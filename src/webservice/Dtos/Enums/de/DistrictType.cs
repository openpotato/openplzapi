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

namespace OpenPlzApi.DE
{
    /// <summary>
    /// District type (Kreiskennzeichen)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public enum DistrictType
    {
        /// <summary>
        /// No type available
        /// </summary>
        None = 0,

        /// <summary>
        /// Kreisfreie Stadt
        /// </summary>
        Kreisfreie_Stadt = 41,

        /// <summary>
        /// Stadtkreis
        /// </summary>
        Stadtkreis = 42,

        /// <summary>
        /// Kreis
        /// </summary>
        Kreis = 43,

        /// <summary>
        /// Landkreis
        /// </summary>
        Landkreis = 44,

        /// <summary>
        /// Regionalverband
        /// </summary>
        Regionalverband = 45
    }
}
