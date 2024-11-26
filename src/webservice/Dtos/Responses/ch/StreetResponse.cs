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

using OpenPlzApi.DataLayer.CH;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.CH
{
    /// <summary>
    /// Representation of a Swiss street (Straße)
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
            Canton = street.Locality?.Commune?.District?.Canton != null ? new CantonSummary(street.Locality?.Commune.District.Canton) : null;
            Commune = street.Locality?.Commune != null ? new CommuneSummary(street.Locality?.Commune) : null;
            District = street.Locality?.Commune?.District != null ? new DistrictSummary(street.Locality?.Commune.District) : null;
            Key = street.Key;
            Locality = street.Locality?.Name;
            Name = street.Name;
            PostalCode = street.Locality?.PostalCode;
            Status = (StreetStatus)street.Status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreetResponse"/> class.
        /// </summary>
        /// <param name="street">Assigns data from <see cref="FullTextStreet"/></param>
        public StreetResponse(FullTextStreet street)
            : base(street)
        {
            Canton = street.Commune?.District?.Canton != null ? new CantonSummary(street.Commune.District.Canton) : null;
            Commune = street.Commune != null ? new CommuneSummary(street.Commune) : null;
            District = street.Commune?.District != null ? new DistrictSummary(street.Commune.District) : null;
            Key = street.Key;
            Locality = street.Locality;
            Name = street.Name;
            PostalCode = street.PostalCode;
            Status = (StreetStatus)street.Status;
        }

        /// <summary>
        /// Reference to canton (Kanton)
        /// </summary>
        [Required]
        [JsonPropertyOrder(7)]
        public CantonSummary Canton { get; }

        /// <summary>
        /// Reference to commune (Gemeinde)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public CommuneSummary Commune { get; }

        /// <summary>
        /// Reference to district (Bezirk)
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public DistrictSummary District { get; }

        /// <summary>
        /// Key (Straßenschlüssel)
        /// </summary>
        /// <example>10023770</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Locality (Ortsname)
        /// </summary>
        /// <example>Grellingen</example>
        [Required]
        [JsonPropertyOrder(4)]
        public string Locality { get; }

        /// <summary>
        /// Name (Straßenname)
        /// </summary>
        /// <example>Wiedenweg</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        /// <example>4203</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string PostalCode { get; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Real</example>
        [Required]
        [JsonPropertyOrder(8)]
        public StreetStatus Status { get; }
    }
}
