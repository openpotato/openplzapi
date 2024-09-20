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

namespace OpenPlzApi.DataLayer.LI
{
    /// <summary>
    /// Representation of a Liechtenstein commune (Gemeinde)
    /// </summary>
    [Table(DbTables.LI.Commune, Schema = DbSchemas.LI)]
    [Index(nameof(Key), IsUnique = true)]
    [Comment("Representation of a Liechtenstein commune (Gemeinde)")]
    public class Commune : BaseEntity
    {
        /// <summary>
        /// Electoral district (Wahlkreis)
        /// </summary>
        [Required]
        [Comment("Electoral district (Wahlkreis)")]
        public string ElectoralDistrict { get; set; }

        /// <summary>
        /// Key (Gemeindenummer)
        /// </summary>
        [Required]
        [Comment("Key (Gemeindenummer)")]
        public string Key { get; set; }

        /// <summary>
        /// Name (Amtlicher Gemeindename)
        /// </summary>
        [Required]
        [Comment("Name (Amtlicher Gemeindename)")]
        public string Name { get; set; }
    }
}
