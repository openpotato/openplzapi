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

using OpenPlzApi.DataLayer.AT;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.AT
{
    /// <summary>
    /// Reduced representation of an Austrian federal province (Bundesland)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class FederalProvinceSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FederalProvinceSummary"/> class.
        /// </summary>
        /// <param name="federalProvince">Assigns data from <see cref="FederalProvince"/></param>
        public FederalProvinceSummary(FederalProvince federalProvince)
        {
            Key = federalProvince.Key;
            Name = federalProvince.Name;
        }

        /// <summary>
        /// Key (Bundeslandkennziffer)
        /// </summary>
        /// <example>9</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Name (Bundeslandname)
        /// </summary>
        /// <example>Wien</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }
    }
}
