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
    /// Reduced representation of a German government region (Regierungsbezirk)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class GovernmentRegionSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GovernmentRegionSummary"/> class.
        /// </summary>
        /// <param name="governmentRegion">Assigns data from <see cref="GovernmentRegion"/></param>
        public GovernmentRegionSummary(GovernmentRegion governmentRegion)
        {
            Key = governmentRegion.RegionalKey;
            Name = governmentRegion.Name;
        }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        /// <example>071</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Name (Bezirksname)
        /// </summary>
        /// <example>früher: Reg.-Bez. Koblenz</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }
    }
}
