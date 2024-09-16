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

using OpenPlzApi.DataLayer.AT;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.AT
{
    /// <summary>
    /// Representation of an Austrian locality (Ortschaft)
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
            District = locality.Municipality?.District != null ? new DistrictSummary(locality.Municipality?.District) : null;
            FederalProvince = locality.Municipality?.District?.FederalProvince != null ? new FederalProvinceSummary(locality.Municipality.District.FederalProvince) : null;
            Key = locality.Key;
            Municipality = locality.Municipality != null ? new MunicipalitySummary(locality.Municipality) : null;
            Name = locality.Name;
            PostalCode = locality.PostalCode;
        }

        /// <summary>
        /// Reference to district (Bezirk)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public DistrictSummary District { get; }

        /// <summary>
        /// Reference to federal province (Bunudesland)
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public FederalProvinceSummary FederalProvince { get; }

        /// <summary>
        /// Key (Ortschaftskennziffer)
        /// </summary>
        /// <example>17224</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Reference to municipality (Gemeinde)
        /// </summary>
        [Required]
        [JsonPropertyOrder(4)]
        public MunicipalitySummary Municipality { get; }

        /// <summary>
        /// Name (Ortschaftsname)
        /// </summary>
        /// <example>Wien, Leopoldstadt</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        /// <example>1020</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string PostalCode { get; }
    }
}
