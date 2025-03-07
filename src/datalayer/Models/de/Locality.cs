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

using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPlzApi.DataLayer.DE
{
    /// <summary>
    /// Representation of a German locality (Ort oder Stadt)
    /// </summary>
    [Table(DbTables.DE.Locality, Schema = DbSchemas.DE)]
    [Index(nameof(MunicipalityId), nameof(PostalCode), nameof(Name), IsUnique = true)]
    [Comment("Representation of a German locality (Ort oder Stadt)")]
    public class Locality : BaseEntity
    {
        /// <summary>
        /// Reference to municipality
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Name (Ortsname)
        /// </summary>
        [Required]
        [Comment("Name (Ortsname)")]
        public string Name { get; set; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        [Required]
        [Comment("Postal code (Postleitzahl)")]
        public string PostalCode { get; set; }

        #region Foreign keys
        [Comment("Reference to municipality (Gemeinde)")]
        public Guid? MunicipalityId { get; set; }
        #endregion Foreign keys
    }
}