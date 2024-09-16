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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPlzApi.DataLayer.CH
{
    /// <summary>
    /// Representation of a Swiss canton (Kanton)
    /// </summary>
    [Table(DbTables.CH.Canton, Schema = DbSchemas.CH)]
    [Index(nameof(Key), IsUnique = true)]
    [Comment("Representation of a Swiss canton (Kanton)")]
    public class Canton : BaseEntity
    {
        /// <summary>
        /// Code (Kantonskürzel)
        /// </summary>
        [Required]
        [Comment("Code (Kantonskürzel)")]
        public string Code { get; set; }

        /// <summary>
        /// Key (Kantonsnummer)
        /// </summary>
        [Required]
        [Comment("Key (Kantonsnummer)")]
        public string Key { get; set; }

        /// <summary>
        /// Name (Kantonsname)
        /// </summary>
        [Required]
        [Comment("Name (Kantonsname)")]
        public string Name { get; set; }
    }
}
