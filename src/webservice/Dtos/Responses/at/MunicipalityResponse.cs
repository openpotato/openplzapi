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
            Key = municipality.Key;
            Code = municipality.Code;
            Name = municipality.Name;
            MultiplePostalCodes = municipality.MultiplePostalCodes;
            PostalCode = municipality.PostalCode;
            Status = (MunicipalityStatus)municipality.Status;
            DistrictKey = municipality.District?.Key;
        }

        /// <summary>
        /// Code (Gemeindecode)
        /// </summary>
        /// <example>10101</example>
        [Required]
        [JsonPropertyOrder(5)]
        public string Code { get; }

        /// <summary>
        /// Reference to district
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public string DistrictKey { get; }

        /// <summary>
        /// Key (Gemeindekennziffer)
        /// </summary>
        /// <example>10101</example>
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
        /// <example>Eisenstadt</example>
        [Required]
        [JsonPropertyOrder(6)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl des Gemeindeamtes)
        /// </summary>
        /// <example>7000</example>
        [Required]
        [JsonPropertyOrder(8)]
        public string PostalCode { get; }

        /// <summary>
        /// Status (Gemeindestatus)
        /// </summary>
        /// <example>TownWithCharter</example>
        [Required]
        [JsonPropertyOrder(10)]
        public MunicipalityStatus Status { get; }
    }
}
