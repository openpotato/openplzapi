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
    /// Municipal association type (Gemeindeverbandskennzeichen)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public enum MunicipalAssociationType
    {
        /// <summary>
        /// No type available
        /// </summary>
        None = 0,

        /// <summary>
        /// Verbandsfreie Gemeinde
        /// </summary>
        Verbandsfreie_Gemeinde = 50,

        /// <summary>
        /// Amt
        /// </summary>
        Amt = 51,

        /// <summary>
        /// Samtgemeinde
        /// </summary>
        Samtgemeinde = 52,

        /// <summary>
        /// Verbandsgemeinde
        /// </summary>
        Verbandsgemeinde = 53,

        /// <summary>
        /// Verwaltungsgemeinschaft
        /// </summary>
        Verwaltungsgemeinschaft = 54,

        /// <summary>
        /// Kirchspielslandgemeinde
        /// </summary>
        Kirchspielslandgemeinde = 55,

        /// <summary>
        /// Verwaltungsverband
        /// </summary>
        Verwaltungsverband = 56,

        /// <summary>
        /// VG-Trägermodell
        /// </summary>
        VG_Traegermodell = 57,

        /// <summary>
        /// Erfüllende Gemeinde
        /// </summary>
        Erfüllende_Gemeinde = 58
    }
}
