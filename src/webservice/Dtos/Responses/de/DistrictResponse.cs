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
    /// Representation of a German district (Kreis)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class DistrictResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictResponse"/> class.
        /// </summary>
        /// <param name="district">Assigns data from <see cref="District"/></param>
        public DistrictResponse(District district)
            : base(district)
        {
            AdministrativeHeadquarters = district.AdministrativeHeadquarters;
            FederalState = district.FederalState != null ? new FederalStateSummary(district.FederalState) : null;
            GovernmentRegion = district.GovernmentRegion != null ? new GovernmentRegionSummary(district.GovernmentRegion) : null;
            Key = district.RegionalKey;
            Name = district.Name;
            Type = (DistrictType)district.Type;
        }

        /// <summary>
        /// Administrative headquarters (Sitz der Kreisverwaltung)
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public string AdministrativeHeadquarters { get; }

        /// <summary>
        /// Reference to federal state (Bundesland)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public FederalStateSummary FederalState { get; }

        /// <summary>
        /// Reference to government region (Regierungsbezirk)
        /// </summary>
        [JsonPropertyOrder(4)]
        public GovernmentRegionSummary GovernmentRegion { get; }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        /// <example>07137</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Name (Kreisname)
        /// </summary>
        /// <example>Mayen-Koblenz</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }

        /// <summary>
        /// Type (Kennzeichen)
        /// </summary>
        /// <example>Landkreis</example>
        [Required]
        [JsonPropertyOrder(3)]
        public DistrictType Type { get; }
    }
}
