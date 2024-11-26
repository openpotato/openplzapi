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

using System.ComponentModel.DataAnnotations;

namespace OpenPlzApi.DataLayer.DE
{
    /// <summary>
    /// Municipal association type (Gemeindeverbandskennzeichen)
    /// </summary>
    public enum MunicipalAssociationType
    {
        [Display(Name = "Amt")]
        Amt = 51,

        [Display(Name = "Samtgemeinde")]
        Samtgemeinde = 52,

        [Display(Name = "Verbandsgemeinde")]
        Verbandsgemeinde = 53, 

        [Display(Name = "Verwaltungsgemeinschaft")]
        Verwaltungsgemeinschaft = 54,

        [Display(Name = "Kirchspielslandgemeinde")]
        Kirchspielslandgemeinde = 55,

        [Display(Name = "Verwaltungsverband")]
        Verwaltungsverband = 56,

        [Display(Name = "VG Trägermodell")]
        VGTraegermodell = 57,

        [Display(Name = "Erfüllende Gemeinde")]
        ErfüllendeGemeinde = 58
    }
}
