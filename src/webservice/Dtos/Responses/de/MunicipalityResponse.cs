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
    /// Representation of a German municipality (Gemeinde)
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
            Key = municipality.RegionalKey;
            Name = municipality.Name;
            Type = (MunicipalityType)municipality.Type;
            PostalCode = municipality.PostalCode;
            MultiplePostalCodes = municipality.MultiplePostalCodes;
            DistrictKey = municipality.District?.RegionalKey;
            AssociationKey = municipality.Association?.RegionalKey;
        }

        /// <summary>
        /// Reference to association
        /// </summary>
        [JsonPropertyOrder(9)]
        public string AssociationKey { get; }

        /// <summary>
        /// Reference to district
        /// </summary>
        [Required]
        [JsonPropertyOrder(8)]
        public string DistrictKey { get; }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public string Key { get; }

        /// <summary>
        /// Multiple postal codes available? 
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public bool MultiplePostalCodes { get; }

        /// <summary>
        /// Name (Gemeindename)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public string Name { get; }

        /// <summary>
        /// Postal code of the administrative headquarters (Verwaltungssitz), if there are multiple postal codes available
        /// </summary>
        [Required]
        [JsonPropertyOrder(4)]
        public string PostalCode { get; }

        /// <summary>
        /// Type (Kennzeichen)
        /// </summary>
        [Required]
        [JsonPropertyOrder(10)]
        public MunicipalityType Type { get; }
    }
}
