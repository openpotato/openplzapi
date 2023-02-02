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
    /// Representation of a German government region (Regierungsbezirk)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class GovernmentRegionResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GovernmentRegionResponse"/> class.
        /// </summary>
        /// <param name="governmentRegion">Assigns data from <see cref="GovernmentRegion"/></param>
        public GovernmentRegionResponse(GovernmentRegion governmentRegion)
            : base(governmentRegion)
        {
            Key = governmentRegion.RegionalKey;
            Name = governmentRegion.Name;
            AdministrativeHeadquarters = governmentRegion.AdministrativeHeadquarters;
            FederalStateKey = governmentRegion.FederalState?.RegionalKey;
        }

        /// <summary>
        /// Administrative headquarters (Verwaltungssitz des Regierungsbezirks)
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public string AdministrativeHeadquarters { get; }

        /// <summary>
        /// Reference to federal state
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public string FederalStateKey { get; }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public string Key { get; }

        /// <summary>
        /// Name (Bezirksname)
        /// </summary>
        [Required]
        [JsonPropertyOrder(4)]
        public string Name { get; }
    }
}
