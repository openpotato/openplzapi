
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

using OpenPlzApi.DataLayer.CH;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.CH
{
    /// <summary>
    /// Reduced representation of a Swiss district (Bezirk)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class DistrictSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictSummary"/> class.
        /// </summary>
        /// <param name="district">Assigns data from <see cref="District"/></param>
        public DistrictSummary(District district)
        {
            Key = district.Key;
            Name = district.Name;
            ShortName = district.ShortName;
        }

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

        /// <summary>
        /// Short name (Bezirksname, kurz)
        /// </summary>
        /// <example>Laufen</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string ShortName { get; }
    }
}
