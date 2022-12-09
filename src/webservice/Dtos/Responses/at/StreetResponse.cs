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

using OpenPlzApi.DataLayer.AT;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.AT
{
    /// <summary>
    /// Representation of an Austrian street (Straße)
    /// </summary>
    public class StreetResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreetResponse"/> class.
        /// </summary>
        /// <param name="street">Assigns data from <see cref="Street"/></param>
        public StreetResponse(Street street)
            : base(street)
        {
            Key = street.Key;
            Name = street.Name;
            Locality = street.Locality != null ? new LocalityResponse(street.Locality) : null;
        }

        /// <summary>
        /// Key (Straßenkennziffer)
        /// </summary>
        /// <example>000001</example>
        [Required]
        [JsonPropertyOrder(4)]
        public string Key { get; }

        /// <summary>
        /// Reference to locality
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public LocalityResponse Locality { get; }

        /// <summary>
        /// Name (Straßenname)
        /// </summary>
        /// <example>Josef Stanislaus Albach-Gasse</example>
        [Required]
        [JsonPropertyOrder(5)]
        public string Name { get; }
    }
}
