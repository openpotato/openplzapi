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

using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPlzApi.DataLayer.CH
{
    /// <summary>
    /// Representation of a Swiss street (Straße)
    /// </summary>
    [Table(DbTables.CH.Street, Schema = DbSchemas.CH)]
    [Index(nameof(Key), nameof(LocalityId), IsUnique = true)]
    [Comment("Representation of a Swiss street (Straße)")]
    public class Street : BaseEntity
    {
        /// <summary>
        /// Key (Straßenschlüssel)
        /// </summary>
        [Required]
        [Comment("Key (Straßenschlüssel)")]
        public string Key { get; set; }

        /// <summary>
        /// Reference to locality
        /// </summary>
        public virtual Locality Locality { get; set; }

        /// <summary>
        /// Name (Straßenname)
        /// </summary>
        [Required]
        [Comment("Name (Straßenname)")]
        public string Name { get; set; }

        /// <summary>
        /// Status (Straßenstatus)
        /// </summary>
        [Comment("Status (Straßenstatus)")]
        public StreetStatus Status { get; set; }

        #region Foreign keys
        [Comment("Reference to locality (Ort oder Stadt)")]
        public Guid LocalityId { get; set; }
        #endregion Foreign keys
    }
}