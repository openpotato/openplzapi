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
    /// Municipality type (Gemeindekennzeichen)
    /// </summary>
    public enum MunicipalityType
    {
        [Display(Name = "Markt")]
        Markt = 60,

        [Display(Name = "Kreisfreie Stadt")]
        KreisfreieStadt = 61,

        [Display(Name = "Stadtkreis")]
        Stadtkreis = 62,

        [Display(Name = "Stadt")]
        Stadt = 63,

        [Display(Name = "Kreisangehörige Gemeinde")]
        KreisangehörigeGemeinde = 64,

        [Display(Name = "Gemeindefreies Gebiet (bewohnt)")]
        GemeindefreiesGebietBewohnt = 65,

        [Display(Name = "Gemeindefreies Gebiet (unbewohnt)")]
        GemeindefreiesGebietUnbewohnt = 66,

        [Display(Name = "Große Kreisstadt")]
        GroßeKreisstadt = 67 
    }
}
