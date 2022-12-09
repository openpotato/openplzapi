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

namespace OpenPlzApi.DataLayer.AT
{
    /// <summary>
    /// Representation of an Austrian municipality (Gemeinde)
    /// </summary>
    [Table(DbTables.AT.Municipality, Schema = DbSchemas.AT)]
    [Index(nameof(Code), IsUnique = true)]
    [Comment("Representation of an Austrian municipality (Gemeinde)")]
    public class Municipality : BaseEntity
    {
        /// <summary>
        /// Code (Gemeindecode)
        /// </summary>
        [Comment("Code (Gemeindecode)")]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Reference to district
        /// </summary>
        public virtual District District { get; set; }

        /// <summary>
        /// Key (Gemeindekennziffer)
        /// </summary>
        [Comment("Key (Gemeindekennziffer)")]
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// This municipality has multiple postal codes?
        /// </summary>
        [Comment("This municipality has multiple postal codes?")]
        [Required]
        public bool MultiplePostalCodes { get; set; }

        /// <summary>
        /// Name (Ortschaftsname)
        /// </summary>
        [Required]
        [Comment("Name (Ortschaftsname)")]
        public string Name { get; set; }

        /// <summary>
        /// Postal code (Postleitzahl des Gemeindeamtes)
        /// </summary>
        [Comment("Postal code (Postleitzahl des Gemeindeamtes)")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Status (Gemeindestatus)
        /// </summary>
        [Required]
        [Comment("Status (Gemeindestatus)")]
        public MunicipalityStatus Status { get; set; }

        #region Foreign keys
        [Comment("Reference to district")]
        public Guid DistrictId { get; set; }
        #endregion Foreign keys
    }
}
