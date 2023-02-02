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

namespace OpenPlzApi.DataLayer.DE
{
    /// <summary>
    /// Representation of a German municipality (Gemeinde)
    /// </summary>
    [Table(DbTables.DE.Municipality, Schema = DbSchemas.DE)]
    [Index(nameof(RegionalKey), IsUnique = true)]
    [Comment("Representation of a German municipality (Gemeinde)")]
    public class Municipality : BaseEntity
    {
        /// <summary>
        /// Reference to municipal association
        /// </summary>
        public virtual MunicipalAssociation Association { get; set; }

        /// <summary>
        /// Reference to district
        /// </summary>
        public virtual District District { get; set; }

        /// <summary>
        /// Multiple postcodes available? 
        /// </summary>
        [Required]
        [Comment("Multiple postcodes available?")]
        public bool MultiplePostalCodes { get; set; }

        /// <summary>
        /// Name (Gemeindename)
        /// </summary>
        [Required]
        [Comment("Name (Gemeindename)")]
        public string Name { get; set; }

        /// <summary>
        /// Postal code of the administrative headquarters (Verwaltungssitz), if there are multiple postal codes available
        /// </summary>
        [Required]
        [Comment("Postal code of the administrative headquarters (Verwaltungssitz), if there are multiple postal codes available")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        [Required]
        [Comment("Regional key (Regionalschlüssel)")]
        public string RegionalKey { get; set; }

        /// <summary>
        /// Short Name (Verkürzter Gemeindename)
        /// </summary>
        [Required]
        [Comment("Short Name (Verkürzter Gemeindename)")]
        public string ShortName { get; set; }

        /// <summary>
        /// Type (Kennzeichen)
        /// </summary>
        [Comment("Type (Gemeindekennzeichen)")]
        public MunicipalityType Type { get; set; }

        #region Foreign keys
        [Comment("Reference to municipal association (Gemeindeverband)")]
        public Guid? AssociationId { get; set; }
        [Comment("Reference to district (Kreis)")]
        public Guid? DistrictId { get; set; }
        #endregion Foreign keys
    }
}
