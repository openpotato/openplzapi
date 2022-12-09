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
    /// Representation of a German district (Kreis)
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
            Key = district.RegionalKey;
            Name = district.Name;
            Type = (DistrictType)district.Type;
            AdministrativeHeadquarters = district.AdministrativeHeadquarters;
            FederalStateKey = district.FederalState?.RegionalKey;
            GovernmentRegionKey = district.GovernmentRegion?.RegionalKey;
        }

        /// <summary>
        /// Administrative headquarters (Sitz der Kreisverwaltung)
        /// </summary>
        [Required]
        [JsonPropertyOrder(8)]
        public string AdministrativeHeadquarters { get; }

        /// <summary>
        /// Reference to federal state 
        /// </summary>
        [Required]
        [JsonPropertyOrder(6)]
        public string FederalStateKey { get; }

        /// <summary>
        /// Reference to government region 
        /// </summary>
        [JsonPropertyOrder(7)]
        public string GovernmentRegionKey { get; }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public string Key { get; }

        /// <summary>
        /// Name (Kreisname)
        /// </summary>
        [Required]
        [JsonPropertyOrder(4)]
        public string Name { get; }

        /// <summary>
        /// Type (Kennzeichen)
        /// </summary>
        [Required]
        [JsonPropertyOrder(9)]
        public DistrictType Type { get; }
    }
}
