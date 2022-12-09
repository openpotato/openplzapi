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

namespace OpenPlzApi.DataLayer.DE
{
    /// <summary>
    /// Municipality type (Gemeindekennzeichen)
    /// </summary>
    public enum MunicipalityType
    {
        None = 0,
        Markt = 60,
        KreisfreieStadt = 61, 
        Stadtkreis = 62,
        Stadt = 63, 
        KreisangehörigeGemeinde = 64,
        GemeindefreiesGebietBewohnt = 65,
        GemeindefreiesGebietUnbewohnt = 66,
        GroßeKreisstadt = 67 
    }
}
