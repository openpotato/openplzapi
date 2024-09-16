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

using Microsoft.EntityFrameworkCore;
using OpenPlzApi.DataLayer.CH;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.CH
{
    /// <summary>
    /// Representation of a Swiss commune (Gemeinde)
    /// </summary>
    public class CommuneResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommuneResponse"/> class.
        /// </summary>
        /// <param name="commune">Assigns data from <see cref="Commune"/></param>
        public CommuneResponse(Commune commune)
            : base(commune)
        {
            Canton = commune.District?.Canton != null ? new CantonSummary(commune.District.Canton) : null;
            District = commune.District != null ? new DistrictSummary(commune.District) : null;
            Key = commune.Key;
            Name = commune.Name;
            ShortName = commune.ShortName;
        }

        /// <summary>
        /// Reference to canton (Kanton)
        /// </summary>
        [Required]
        [JsonPropertyOrder(5)]
        public CantonSummary Canton { get; }

        /// <summary>
        /// Reference to district (Bezirk)
        /// </summary>
        [Required]
        [JsonPropertyOrder(4)]
        public DistrictSummary District { get; }

        /// <summary>
        /// Key (Gemeindenummer)
        /// </summary>
        /// <example>2786</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Name (Amtlicher Gemeindename)
        /// </summary>
        /// <example>Grellingen</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }

        /// <summary>
        /// Short name (Gemeindename, kurz)
        /// </summary>
        /// <example>Grellingen</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string ShortName { get; }
    }
}
