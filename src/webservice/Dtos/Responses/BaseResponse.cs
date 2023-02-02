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

using OpenPlzApi.DataLayer;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenPlzApi
{
    /// <summary>
    /// Abstract response
    /// </summary>
    public abstract class BaseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponse"/> class.
        /// </summary>
        /// <param name="entity">Assigns data from <see cref="BaseEntity"/> based instance</param>
        public BaseResponse(BaseEntity entity)
        {
            Source = entity.Source;
            TimeStamp = entity.TimeStamp;
        }

        /// <summary>
        /// From where was this entity imported?
        /// </summary>
        [Required]
        [JsonPropertyOrder(2)]
        public string Source { get; }

        /// <summary>
        /// When was this entity imported?
        /// </summary>
        [Required]
        [JsonPropertyOrder(3)]
        public DateOnly TimeStamp { get; }
    }
}
