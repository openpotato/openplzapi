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
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.AT
{
    /// <summary>
    /// Representation of an Austrian street (Straße)
    /// </summary>
    public class StreetResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreetResponse"/> class.
        /// </summary>
        /// <param name="street">Assigns data from <see cref="Street"/></param>
        public StreetResponse(Street street)
            : base(street)
        {
            District = street.Locality?.Municipality?.District != null ? new DistrictSummary(street.Locality.Municipality?.District) : null;
            FederalProvince = street.Locality?.Municipality?.District?.FederalProvince != null ? new FederalProvinceSummary(street.Locality.Municipality.District.FederalProvince) : null;
            Key = street.Key;
            Locality = street.Locality?.Name;
            Municipality = street.Locality?.Municipality != null ? new MunicipalitySummary(street.Locality.Municipality) : null;
            Name = street.Name;
            PostalCode = street.Locality?.PostalCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreetResponse"/> class.
        /// </summary>
        /// <param name="street">Assigns data from <see cref="FullTextStreet"/></param>
        public StreetResponse(FullTextStreet street)
            : base(street)
        {
            District = street.Municipality?.District != null ? new DistrictSummary(street.Municipality?.District) : null;
            FederalProvince = street.Municipality?.District?.FederalProvince != null ? new FederalProvinceSummary(street.Municipality.District.FederalProvince) : null;
            Key = street.Key;
            Locality = street.Locality;
            Municipality = street.Municipality != null ? new MunicipalitySummary(street.Municipality) : null;
            Name = street.Name;
            PostalCode = street.PostalCode;
        }

        /// <summary>
        /// Reference to district (Bezirk)
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public DistrictSummary District { get; }

        /// <summary>
        /// Reference to federal province (Bundesland)
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public FederalProvinceSummary FederalProvince { get; }

        /// <summary>
        /// Key (Straßenkennziffer)
        /// </summary>
        /// <example>900017</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Locality (Ortschaftsname)
        /// </summary>
        /// <example>Wien, Leopoldstadt</example>
        [Required]
        [JsonPropertyOrder(4)]
        public string Locality { get; }

        /// <summary>
        /// Reference to municipality (Gemeinde)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public MunicipalitySummary Municipality { get; }

        /// <summary>
        /// Name (Straßenname)
        /// </summary>
        /// <example>Adambergergasse</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        /// <example>1020</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string PostalCode { get; }
    }
}
