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

using System;
using System.Collections.Generic;

namespace OpenPlzApi.CLI.Sources.CH
{
    /// <summary>
    /// Swiss street (Straße))
    /// </summary>
    public class Street : BaseRecord
    {
        /// <summary>
        /// Date of last modification
        /// </summary>
        public DateOnly LastModified { get; set; }

        /// <summary>
        /// Reference to locality
        /// </summary>
        public List<_Locality> Localities { get; set; } = new();

        /// <summary>
        /// Official address?
        /// </summary>
        public bool Official { get; set; }
        
        /// <summary>
        /// Street status
        /// </summary>
        public StreetStatus Status { get; set; }

        /// <summary>
        /// Street type
        /// </summary>
        public StreetType Type { get; set; }

        /// <summary>
        /// Embedded locality (Ort oder Stadt)
        /// </summary>
        public class _Locality
        {
            /// <summary>
            /// Reference to commune
            /// </summary>
            public _Commune Commune { get; set; }

            /// <summary>
            /// Name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Postal code
            /// </summary>
            public string PostalCode { get; set; }

            /// <summary>
            /// Get a predictable unique id for this locality
            /// </summary>
            /// <returns>A guid value</returns>
            public Guid GetUniqueId()
            {
                return IdFactory.CreateIdFromValue($"{PostalCode}.{Name}");
            }

            /// <summary>
            /// Embedded commune (Gemeinde)
            /// </summary>
            public class _Commune
            {
                /// <summary>
                /// Code of canton
                /// </summary>
                public string Canton { get; set; }

                /// <summary>
                /// Key
                /// </summary>
                public string Key { get; set; }

                /// <summary>
                /// Name
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// Get a predictable unique id for this commune
                /// </summary>
                /// <returns>A guid value</returns>
                public Guid GetUniqueId()
                {
                    return IdFactory.CreateIdFromValue(Key);
                }
            }
        }
    }
}
