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
    /// Representation of a German government region (Regierungsbezirk)
    /// </summary>
    [Table(DbTables.DE.GovernmentRegion, Schema = DbSchemas.DE)]
    [Index(nameof(RegionalKey), IsUnique = true)]
    [Comment("Representation of a German government region (Regierungsbezirk)")]
    public class GovernmentRegion : BaseEntity
    {
        /// <summary>
        /// Administrative headquarters (Verwaltungssitz des Regierungsbezirks)
        /// </summary>
        [Comment("Administrative headquarters (Verwaltungssitz des Regierungsbezirks)")]
        public string AdministrativeHeadquarters { get; set; }

        /// <summary>
        /// Reference to federal state
        /// </summary>
        public virtual FederalState FederalState { get; set; }

        /// <summary>
        /// Name (Bezirksname)
        /// </summary>
        [Required]
        [Comment("Name (Bezirksname)")]
        public string Name { get; set; }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        [Required]
        [Comment("Regional key (Regionalschlüssel)")]
        public string RegionalKey { get; set; }

        #region Foreign keys
        [Comment("Reference to federal state (Bundesland)")]
        public Guid FederalStateId { get; set; }
        #endregion Foreign keys
    }
}
