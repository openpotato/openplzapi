#region OpenPLZ API - Copyright (C) 2022 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2022 STÜBER SYSTEMS GmbH
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
            Key = locality.Key;
            Name = locality.Name;
            PostalCode = locality.PostalCode;
            MunicipalityKey = locality.Municipality?.Key;
        }

        /// <summary>
        /// Key (Ortschaftskennziffer)
        /// </summary>
        /// <example>00001</example>
        [Required]
        [JsonPropertyOrder(4)]
        public string Key { get; }

        /// <summary>
        /// Reference to municipality
        /// </summary>
        /// <example>10101</example>
        [Required]
        [JsonPropertyOrder(7)]
        public string MunicipalityKey { get; }

        /// <summary>
        /// Name (Ortschaftsname)
        /// </summary>
        /// <example>Eisenstadt</example>
        [Required]
        [JsonPropertyOrder(6)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        /// <example>7000</example>
        [Required]
        [JsonPropertyOrder(5)]
        public string PostalCode { get; }
    }
}
