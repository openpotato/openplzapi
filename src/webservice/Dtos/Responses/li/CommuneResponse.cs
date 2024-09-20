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

using OpenPlzApi.DataLayer.LI;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.LI
{
    /// <summary>
    /// Representation of a Liechtenstein commune (Gemeinde)
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
            Key = commune.Key;
            Name = commune.Name;
            ElectoralDistrict = commune.ElectoralDistrict;
        }

        /// <summary>
        /// Key (Gemeindenummer)
        /// </summary>
        /// <example>7005</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Name (Amtlicher Gemeindename)
        /// </summary>
        /// <example>Schaan</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Name { get; }

        /// <summary>
        /// Electoral district (Wahlkreis)
        /// </summary>
        /// <example>Oberland</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string ElectoralDistrict { get; }
    }
}
