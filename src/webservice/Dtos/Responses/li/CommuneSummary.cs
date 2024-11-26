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
    /// Reduced representation of a Liechtenstein commune (Gemeinde)
    /// </summary>
    public class CommuneSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommuneSummary"/> class.
        /// </summary>
        /// <param name="commune">Assigns data from <see cref="Commune"/></param>
        public CommuneSummary(Commune commune)
        {
            Key = commune.Key;
            Name = commune.Name;
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
    }
}
