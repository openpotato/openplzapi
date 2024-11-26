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
using NpgsqlTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPlzApi.DataLayer.AT
{
    /// <summary>
    /// Representation of an Austrian street (Straße)
    /// </summary>
    /// <remarks>
    /// This entity is a special variant for supporting full text search
    /// </remarks>
    [Table(DbTables.AT.FullTextStreet, Schema = DbSchemas.AT)]
    [Comment("Representation of an Austrian street (Straße) for full text search")]
    public class FullTextStreet : BaseEntity
    {
        /// <summary>
        /// Key (Straßenkennziffer)
        /// </summary>
        [Required]
        [Comment("Key (Straßenkennziffer)")]
        public string Key { get; set; }

        /// <summary>
        /// Locality (Ortschaft)
        /// </summary>
        [Required]
        [Comment("Locality (Ortschaft)")]
        public string Locality { get; set; }

        /// <summary>
        /// Reference to municipality (Gemeinde)
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Name (Straßenname)
        /// </summary>
        [Required]
        [Comment("Name (Straßenname)")]
        public string Name { get; set; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        [Required]
        [Comment("Postal code (Postleitzahl)")]
        public string PostalCode { get; set; }

        /// <summary>
        /// tsvector column for full text search
        /// </summary>
        [Comment("tsvector column for full text search")]
        public NpgsqlTsVector SearchVector { get; set; }

        #region Foreign keys
        [Comment("Reference to municipality")]
        public Guid MunicipalityId { get; set; }
        #endregion Foreign keys
    }
}