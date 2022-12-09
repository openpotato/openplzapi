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
            Municipality = locality.Municipality != null ? new MunicipalityResponse(locality.Municipality) : null;
            Name = locality.Name;
            PostalCode = locality.PostalCode;
        }

        /// <summary>
        /// Reference to municipality
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public MunicipalityResponse Municipality { get; }

        /// <summary>
        /// Name (Ortsname)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        [Required]
        [JsonPropertyOrder(4)]
        public string PostalCode { get; }
    }
}
