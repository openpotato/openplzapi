﻿#region OpenPLZ API - Copyright (c) STÜBER SYSTEMS GmbH
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

namespace OpenPlzApi.CLI.Sources.AT
{
    /// <summary>
    /// Austrian street (Straße)
    /// </summary>
    public class Street : BaseRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Street"/> class.
        /// </summary>
        /// <param name="timeStamp">A time stamp</param>
        public Street(DateOnly timeStamp)
            : base(timeStamp)
        {
        }

        /// <summary>
        /// Reference to locality
        /// </summary>
        public _Locality Locality { get; set; }
        
        public class _Locality
        {
            /// <summary>
            /// Key
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Reference to municipality
            /// </summary>
            public _Municipality Municipality { get; set; }

            /// <summary>
            /// Name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Postal code
            /// </summary>
            public string PostalCode { get; set; }

            /// <summary>
            /// Get a predictable unique id for the locality
            /// </summary>
            /// <returns>A guid value</returns>
            public Guid GetUniqueId()
            {
                return IdFactory.CreateIdFromValue($"{Key}.{PostalCode}.{Municipality.GetUniqueId()}");
            }

            /// <summary>
            /// Embedded municipality (Gemeinde)
            /// </summary>
            public class _Municipality
            {
                /// <summary>
                /// Code
                /// </summary>
                public string Code { get; set; }

                /// <summary>
                /// Key
                /// </summary>
                public string Key { get; set; }

                /// <summary>
                /// Name
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// Get a predictable unique id for the municipality
                /// </summary>
                /// <returns>A guid value</returns>
                public Guid GetUniqueId()
                {
                    return IdFactory.CreateIdFromValue($"{Code}");
                }
            }
        }
    }
}
