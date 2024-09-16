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

namespace OpenPlzApi.DataLayer.AT
{
    /// <summary>
    /// Representation of an Austrian district (Politischer Bezirk)
    /// </summary>
    [Table(DbTables.AT.Districts, Schema = DbSchemas.AT)]
    [Index(nameof(Code), IsUnique = true)]
    [Comment("Representation of an Austrian district (Politischer Bezirk)")]
    public class District : BaseEntity
    {
        /// <summary>
        /// Reference to federal province
        /// </summary>
        public virtual FederalProvince FederalProvince { get; set; }

        /// <summary>
        /// Code (Bezirkskodierung)
        /// </summary>
        [Required]
        [Comment("Code (Bezirkskodierung)")]
        public string Code { get; set; }

        /// <summary>
        /// Key (Bezirkskennziffer)
        /// </summary>
        [Required]
        [Comment("Key (Bezirkskennziffer)")]
        public string Key { get; set; }

        /// <summary>
        /// Name (Bezirksname)
        /// </summary>
        [Required]
        [Comment("Name (Bezirksname)")]
        public string Name { get; set; }

        #region Foreign keys
        [Comment("Reference to federal province (Bundesland)")]
        public Guid FederalProvinceId { get; set; }
        #endregion Foreign keys
    }
}
