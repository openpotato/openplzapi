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
            Association = municipality.Association != null ? new MunicipalAssociationSummary(municipality.Association) : null;
            District = municipality.District != null ? new DistrictSummary(municipality.District) : null;
            FederalState = municipality.FederalState != null ? new FederalStateSummary(municipality.FederalState) : null;
            GovernmentRegion = municipality.District?.GovernmentRegion != null ? new GovernmentRegionSummary(municipality.District.GovernmentRegion) : null;
            Key = municipality.RegionalKey;
            MultiplePostalCodes = municipality.MultiplePostalCodes;
            Name = municipality.Name;
            PostalCode = municipality.PostalCode;
            Type = (MunicipalityType)municipality.Type;
        }

        /// <summary>
        /// Reference to association (Gemeindeverbund)
        /// </summary>
        [JsonPropertyOrder(6)]
        public MunicipalAssociationSummary Association { get; }

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
        [JsonPropertyOrder(9)]
        public FederalStateSummary FederalState { get; }

        /// <summary>
        /// Reference to government region (Regierungsbezirk)
        /// </summary>
        [Required]
        [JsonPropertyOrder(8)]
        public GovernmentRegionSummary GovernmentRegion { get; }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        /// <example>07137203</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Multiple postal codes available? 
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public bool MultiplePostalCodes { get; }

        /// <summary>
        /// Name (Gemeindename)
        /// </summary>
        /// <example>Bendorf, Stadt</example>
        [Required]
        [JsonPropertyOrder(2)]
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
        /// <example>Stadt</example>
        [Required]
        [JsonPropertyOrder(3)]
        public MunicipalityType Type { get; }
    }
}
