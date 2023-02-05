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

using OpenPlzApi.DataLayer.CH;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.CH
{
    /// <summary>
    /// Representation of a Swiss district (Bezirk)
    /// </summary>
    public class DistrictResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictResponse"/> class.
        /// </summary>
        /// <param name="district">Assigns data from <see cref="District"/></param>
        public DistrictResponse(District district)
            : base(district)
        {
            Canton = district.Canton != null ? new CantonSummary(district.Canton) : null;
            Key = district.Key;
            Name = district.Name;
        }

        /// <summary>
        /// Reference to canton (Kanton)
        /// </summary>
        [Required]
        [JsonPropertyOrder(3)]
        public CantonSummary Canton { get; }

        /// <summary>
        /// Key (Bezirksnummer)
        /// </summary>
        /// <example>1302</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Name (Bezirksname)
        /// </summary>
        /// <example>Bezirk Laufen</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }
    }
}
