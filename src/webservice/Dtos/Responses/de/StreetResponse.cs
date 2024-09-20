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
            District = street.Locality?.Municipality?.District != null ? new DistrictSummary(street.Locality?.Municipality.District) : null;
            FederalState = street.Locality?.Municipality?.FederalState != null ? new FederalStateSummary(street.Locality?.Municipality.FederalState) : null;
            Locality = street.Locality?.Name;
            Municipality = street.Locality?.Municipality != null ? new MunicipalitySummary(street.Locality?.Municipality) : null;
            Name = street.Name;
            PostalCode = street.Locality?.PostalCode;
            Suburb = street.Suburb;
            Borough = street.Borough;
        }

        /// <summary>
        /// Borough (Stadtbezirk)
        /// </summary>
        [Required]
        [JsonPropertyOrder(4)]
        public string Borough { get; }

        /// <summary>
        /// Reference to district (Kreis)
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public DistrictSummary District { get; }

        /// <summary>
        /// Reference to federal state (Bundesland)
        /// </summary>
        [Required]
        [JsonPropertyOrder(8)]
        public FederalStateSummary FederalState { get; }

        /// <summary>
        /// Locality (Ortsname)
        /// </summary>
        /// <example>Bendorf</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string Locality { get; }

        /// <summary>
        /// Reference to municipality
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public MunicipalitySummary Municipality { get; }

        /// <summary>
        /// Name (Straßenname)
        /// </summary>
        /// <example>Engerser Landstr.</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        /// <example>56170</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string PostalCode { get; }

        /// <summary>
        /// Suburb (Stadtteil)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public string Suburb { get; }
    }
}
