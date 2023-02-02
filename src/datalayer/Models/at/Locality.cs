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

namespace OpenPlzApi.DataLayer.AT
{
    /// <summary>
    /// Representation of an Austrian locality (Stadt, Ort)
    /// </summary>
    [Table(DbTables.AT.Locality, Schema = DbSchemas.AT)]
    [Index(nameof(Key), nameof(PostalCode), nameof(MunicipalityId), IsUnique = true)]
    [Comment("Representation of an Austrian locality (Stadt, Ort)")]
    public class Locality : BaseEntity
    {
        /// <summary>
        /// Key (Ortschaftskennziffer)
        /// </summary>
        [Required]
        [Comment("Key (Ortschaftskennziffer)")]
        public string Key { get; set; }

        /// <summary>
        /// Reference to municipality
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Name (Ortschaftsname)
        /// </summary>
        [Required]
        [Comment("Name (Ortschaftsname)")]
        public string Name { get; set; }

        /// <summary>
        /// Postal code (Postleitzahl)
        /// </summary>
        [Required]
        [Comment("Postal code (Postleitzahl)")]
        public string PostalCode { get; set; }

        #region Foreign keys
        [Comment("Reference to municipality")]
        public Guid MunicipalityId { get; set; }
        #endregion Foreign keys
    }
}