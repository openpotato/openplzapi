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
    /// Representation of an Austrian district (Bezirk)
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
            Code = district.Code;
            Key = district.Key;
            Name = district.Name;
            FederalProvinceKey = district.FederalProvince?.Key;
        }

        /// <summary>
        /// Code (Bezirkskodierung)
        /// </summary>
        /// <example>101</example>
        [Required]
        [JsonPropertyOrder(5)]
        public string Code { get; }

        /// <summary>
        /// Reference to federal province
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public string FederalProvinceKey { get; }

        /// <summary>
        /// Key (Bezirkskennziffer)
        /// </summary>
        /// <example>101</example>
        [Required]
        [JsonPropertyOrder(4)]
        public string Key { get; }

        /// <summary>
        /// Name (Bezirksname)
        /// </summary>
        /// <example>Eisenstadt(Stadt)</example>
        [Required]
        [JsonPropertyOrder(6)]
        public string Name { get; }
    }
}
