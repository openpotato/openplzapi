#region OpenPLZ API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
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
    /// Representation of a German street (Straße)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class StreetResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreetResponse"/> class.
        /// </summary>
        /// <param name="street">Assigns data from <see cref="Street"/></param>
        public StreetResponse(Street street)
            : base(street)
        {
            Name = street.Name;
            Locality = street.Locality != null ? new LocalityResponse(street.Locality) : null;
        }

        /// <summary>
        /// Reference to locality
        /// </summary>
        /// <example>10101</example>
        [Required]
        [JsonPropertyOrder(5)]
        public virtual LocalityResponse Locality { get; }

        /// <summary>
        /// Name (Straßenname)
        /// </summary>
        /// <example>10101</example>
        [Required]
        [JsonPropertyOrder(4)]
        public string Name { get; }
    }
}
