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

using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPlzApi.DataLayer.CH
{
    /// <summary>
    /// Representation of a Swiss district (Bezirk)
    /// </summary>
    [Table(DbTables.CH.District, Schema = DbSchemas.CH)]
    [Index(nameof(Key), IsUnique = true)]
    [Comment("Representation of a Swiss district (Bezirk)")]
    public class District : BaseEntity
    {
        /// <summary>
        /// Reference to canton
        /// </summary>
        public virtual Canton Canton { get; set; }

        /// <summary>
        /// Key (Bezirksnummer)
        /// </summary>
        [Required]
        [Comment("Key (Bezirksnummer)")]
        public string Key { get; set; }

        /// <summary>
        /// Name (Bezirksname)
        /// </summary>
        [Required]
        [Comment("Name (Bezirksname)")]
        public string Name { get; set; }

        #region Foreign keys
        [Comment("Reference to canton (Kanton)")]
        public Guid CantonId { get; set; }
        #endregion Foreign keys
    }
}
