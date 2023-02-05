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

using OpenPlzApi.DataLayer.CH;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi.CH
{
    /// <summary>
    /// Representation of a Swiss canton (Kanton)
    /// </summary>
    public class CantonResponse : BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CantonResponse"/> class.
        /// </summary>
        /// <param name="canton">Assigns data from <see cref="Canton"/></param>
        public CantonResponse(Canton canton)
            : base(canton)
        {
            Code = canton.Code;
            Key = canton.Key;
            Name = canton.Name;
        }

        /// <summary>
        /// Code (Kantonskürzel)
        /// </summary>
        /// <example>BL</example>
        [Required]
        [JsonPropertyOrder(2)]
        public string Code { get; }

        /// <summary>
        /// Key (Kantonsnummer)
        /// </summary>
        /// <example>13</example>
        [Required]
        [JsonPropertyOrder(1)]
        public string Key { get; }

        /// <summary>
        /// Name (Kantonsname)
        /// </summary>
        /// <example>Basel-Landschaft</example>
        [Required]
        [JsonPropertyOrder(3)]
        public string Name { get; }
    }
}
