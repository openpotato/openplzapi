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

namespace OpenPlzApi.DataLayer.DE
{
    /// <summary>
    /// Representation of a German street (Straße)
    /// </summary>
    /// <remarks>
    /// This entity is a special variant for supporting full text search
    /// </remarks>
    [Table(DbTables.DE.FullTextStreet, Schema = DbSchemas.DE)]
    [Comment("Representation of a German street (Straße) for full text search")]
    public class FullTextStreet : BaseEntity
    {
        /// <summary>
        /// Borough (Stadtbezirk)
        /// </summary>
        [Comment("Borough (Stadtbezirk)")]
        public string Borough { get; set; }

        /// <summary>
        /// Locality (Ort)
        /// </summary>
        [Required]
        [Comment("Locality (Ort)")]
        public string Locality { get; set; }

        /// <summary>
        /// Reference to municipality
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

        /// <summary>
        /// Suburb (Orts- oder Stadtteil)
        /// </summary>
        [Comment("Suburb (Orts- oder Stadtteil)")]
        public string Suburb { get; set; }

        #region Foreign keys
        [Comment("Reference to municipality (Gemeinde)")]
        public Guid? MunicipalityId { get; set; }
        #endregion Foreign keys
    }
}