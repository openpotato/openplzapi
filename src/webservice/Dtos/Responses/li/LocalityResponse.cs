﻿#region OpenPLZ API - Copyright (c) STÜBER SYSTEMS GmbH
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

using OpenPlzApi.DataLayer.LI;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.LI
{
    /// <summary>
    /// Representation of a Swiss locality (Ort oder Stadt)
    /// </summary>
    public class LocalityResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalityResponse"/> class.
        /// </summary>
        /// <param name="locality">Assigns data from <see cref="Locality"/></param>
        public LocalityResponse(Locality locality)
            : base(locality)
        {
            Commune = locality.Commune != null ? new CommuneSummary(locality.Commune) : null;
            Name = locality.Name;
            PostalCode = locality.PostalCode;
        }

        /// <summary>
        /// Reference to commune (Gemeinde)
        /// </summary>
        [Required]
        [JsonPropertyOrder(3)]
        public CommuneSummary Commune { get; }

        /// <summary>
        /// Name (Ortsname)
        /// </summary>
        /// <example>Grellingen</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        /// <example>4203</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string PostalCode { get; }
    }
}