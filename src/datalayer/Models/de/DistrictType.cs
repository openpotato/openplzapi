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
    /// District type (Kreiskennzeichen)
    /// </summary>
    public enum DistrictType
    {
        [Display(Name = "Kreisfreie Stadt")]
        KreisfreieStadt = 41,

        [Display(Name = "Stadtkreis")]
        Stadtkreis = 42,

        [Display(Name = "Kreis")]
        Kreis = 43,

        [Display(Name = "Landkreis")]
        Landkreis = 44,

        [Display(Name = "Regionalverband")]
        Regionalverband = 45 
    }
}
