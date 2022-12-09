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
    /// Municipality type (Gemeindekennzeichen)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public enum MunicipalityType
    {
        /// <summary>
        /// No type available
        /// </summary>
        None = 0,

        /// <summary>
        /// Markt
        /// </summary>
        Markt = 60,

        /// <summary>
        /// Kreisfreie Stadt
        /// </summary>
        Kreisfreie_Stadt = 61,

        /// <summary>
        /// Stadtkreis
        /// </summary>
        Stadtkreis = 62,

        /// <summary>
        /// Stadt
        /// </summary>
        Stadt = 63,

        /// <summary>
        /// Kreisangehörige Gemeinde
        /// </summary>
        Kreisangehörige_Gemeinde = 64,

        /// <summary>
        /// GemeindefreiesGebiet (Bewohnt)
        /// </summary>
        Gemeindefreies_Gebiet_Bewohnt = 65,

        /// <summary>
        /// Gemeindefreies Gebiet (Unbewohnt)
        /// </summary>
        Gemeindefreies_Gebiet_Unbewohnt = 66,

        /// <summary>
        /// Große Kreisstadt
        /// </summary>
        Große_Kreisstadt = 67
    }
}
