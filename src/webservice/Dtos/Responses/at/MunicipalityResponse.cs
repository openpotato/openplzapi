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
    /// Representation of an Austrian municipality (Gemeinde)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public class MunicipalityResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MunicipalityResponse"/> class.
        /// </summary>
        /// <param name="municipality">Assigns data from <see cref="Municipality"/></param>
        public MunicipalityResponse(Municipality municipality)
            : base(municipality)
        {
            Code = municipality.Code;
            District = new DistrictSummary(municipality.District);
            FederalProvince = new FederalProvinceSummary(municipality.District.FederalProvince);
            Key = municipality.Key;
            MultiplePostalCodes = municipality.MultiplePostalCodes;
            Name = municipality.Name;
            PostalCode = municipality.PostalCode;
            Status = municipality.Status.GetDisplayName();
        }

        /// <summary>
        /// Code (Gemeindecode)
        /// </summary>
        /// <example>90201</example>
        [Required]
        [JsonPropertyOrder(5)]
        public string Code { get; }

        /// <summary>
        /// Reference to district (Bezirk)
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public DistrictSummary District { get; }

        /// <summary>
        /// Reference to federal province (Bunudesland)
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public FederalProvinceSummary FederalProvince { get; }

        /// <summary>
        /// Key (Gemeindekennziffer)
        /// </summary>
        /// <example>90001</example>
        [Required]
        [JsonPropertyOrder(4)]
        public string Key { get; }

        /// <summary>
        /// This municipality has multiple postal codes?
        /// </summary>
        /// <example>false</example>
        [Required]
        [JsonPropertyOrder(9)]
        public bool MultiplePostalCodes { get; }

        /// <summary>
        /// Name (Ortschaftsname)
        /// </summary>
        /// <example>Wien</example>
        [Required]
        [JsonPropertyOrder(6)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl des Gemeindeamtes)
        /// </summary>
        /// <example>1020</example>
        [Required]
        [JsonPropertyOrder(8)]
        public string PostalCode { get; }

        /// <summary>
        /// Status (Gemeindestatus)
        /// </summary>
        /// <example>Statutarstadt</example>
        [Required]
        [JsonPropertyOrder(10)]
        public string Status { get; }
    }
}
