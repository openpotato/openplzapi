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

using OpenPlzApi.DataLayer.DE;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.DE
{
    /// <summary>
    /// Representation of a German federal state (Bundesland)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class FederalStateResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FederalStateResponse"/> class.
        /// </summary>
        /// <param name="federalState">Assigns data from <see cref="FederalState"/></param>
        public FederalStateResponse(FederalState federalState)
            : base(federalState)
        {
            Key = federalState.Key;
            Name = federalState.Name;
            SeatOfGovernment = federalState.SeatOfGovernment;
        }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        /// <example>0</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Name (Bundeslandname)
        /// </summary>
        /// <example>Rheinland-Pfalz</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }

        /// <summary>
        /// Seat of government (Sitz der Landesregierung)
        /// </summary>
        /// <example>Mainz</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string SeatOfGovernment { get; }
    }
}
