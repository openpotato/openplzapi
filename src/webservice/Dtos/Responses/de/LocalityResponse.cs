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
    /// Representation of a German locality (Ort oder Stadt)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class LocalityResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalityResponse"/> class.
        /// </summary>
        /// <param name="locality">Assigns data from <see cref="Locality"/></param>
        public LocalityResponse(Locality locality)
            : base(locality)
        {
            District = locality.Municipality?.District != null ? new DistrictSummary(locality.Municipality.District) : null;
            FederalState = locality.Municipality?.FederalState != null ? new FederalStateSummary(locality.Municipality.FederalState) : null;
            Municipality = new MunicipalitySummary(locality.Municipality);
            Name = locality.Name;
            PostalCode = locality.PostalCode;
        }

        /// <summary>
        /// Reference to district (Kreis)
        /// </summary>
        [Required]
        [JsonPropertyOrder(4)]
        public DistrictSummary District { get; }

        /// <summary>
        /// Reference to federal state (Bundesland)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public FederalStateSummary FederalState { get; }

        /// <summary>
        /// Reference to municipality
        /// </summary>
        [Required]
        [JsonPropertyOrder(3)]
        public MunicipalitySummary Municipality { get; }

        /// <summary>
        /// Name (Ortsname)
        /// </summary>
        /// <example>Bendorf</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        /// <example>56170</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string PostalCode { get; }
    }
}
