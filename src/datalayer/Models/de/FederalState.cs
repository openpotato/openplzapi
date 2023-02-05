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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPlzApi.DataLayer.DE
{
    /// <summary>
    /// Representation of a German federal state (Bundesland)
    /// </summary>
    [Table(DbTables.DE.FederalState, Schema = DbSchemas.DE)]
    [Index(nameof(RegionalKey), IsUnique = true)]
    [Comment("Representation of a German federal state (Bundesland)")]
    public class FederalState : BaseEntity
    {
        /// <summary>
        /// Name (Bundeslandname)
        /// </summary>
        [Required]
        [Comment("Name (Bundeslandname)")]
        public string Name { get; set; }

        /// <summary>
        /// Regional key (Regionalschlüssel)
        /// </summary>
        [Required]
        [Comment("Regional key (Regionalschlüssel)")]
        public string RegionalKey { get; set; }

        /// <summary>
        /// Seat of government (Sitz der Landesregierung)
        /// </summary>
        [Comment("Seat of government (Sitz der Landesregierung)")]
        public string SeatOfGovernment { get; set; }
    }
}
