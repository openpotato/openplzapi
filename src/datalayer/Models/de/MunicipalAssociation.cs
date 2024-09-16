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
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPlzApi.DataLayer.DE
{
    /// <summary>
    /// Representation of a German municipal association (Gemeindeverband)
    /// </summary>
    [Table(DbTables.DE.MunicipalAssociation, Schema = DbSchemas.DE)]
    [Index(nameof(RegionalKey), nameof(Code), IsUnique = true)]
    [Comment("Representation of a German municipal association (Gemeindeverband)")]
    public class MunicipalAssociation : BaseEntity
    {
        /// <summary>
        /// Administrative headquarters (Verwaltungssitz des Gemeindeverbandes)
        /// </summary>
        [Comment("Administrative headquarters (Verwaltungssitz des Gemeindeverbandes)")]
        public string AdministrativeHeadquarters { get; set; }

        /// <summary>
        /// Code (Code des Gemeindeverbandes)
        /// </summary>
        [Required]
        [Comment("Code (Code des Gemeindeverbandes)")]
        public string Code { get; set; }

        /// <summary>
        /// Reference to district
        /// </summary>
        public virtual District District { get; set; }

        /// <summary>
        /// Name (Name des Gemeindeverbandes)
        /// </summary>
        [Required]
        [Comment("Name (Name des Gemeindeverbandes)")]
        public string Name { get; set; }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        [Required]
        [Comment("Regional key (Regionalschlüssel)")]
        public string RegionalKey { get; set; }

        /// <summary>
        /// Type (Kennzeichen)
        /// </summary>
        [Comment("Type (Kennzeichen)")]
        public MunicipalAssociationType Type { get; set; }

        #region Foreign keys
        [Comment("Reference to district (Kreis)")]
        public Guid? DistrictId { get; set; }
        #endregion Foreign keys
    }
}
