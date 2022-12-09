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
            Key = district.Key;
            Name = district.Name;
            CantonKey = district.Canton?.Key;
        }

        /// <summary>
        /// Reference to canton
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public string CantonKey { get; }

        /// <summary>
        /// Key (Bezirksnummer)
        /// </summary>
        /// <example>ZH</example>
        [Required]
        [JsonPropertyOrder(4)]
        public string Key { get; }

        /// <summary>
        /// Name (Bezirksname)
        /// </summary>
        /// <example>ZH</example>
        [Required]
        [JsonPropertyOrder(5)]
        public string Name { get; }
    }
}
