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

using System;

namespace OpenPlzApi.CLI.Sources.DE
{
    /// <summary>
    /// German street (Straße)
    /// </summary>
    public class Street
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Street"/> class.
        /// </summary>
        public Street()
        {
        }

        /// <summary>
        /// Reference to locality
        /// </summary>
        public _Locality Locality { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Embedded locality (Ort oder Stadt)
        /// </summary>
        public class _Locality
        {
            /// <summary>
            /// Municipality
            /// </summary>
            public string MunicipalityKey { get; set; }

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
                return IdFactory.CreateIdFromValue($"{PostalCode}.{Name}.{MunicipalityKey}");
            }

            /// <summary>
            /// Get a predictable unique id for the municipality
            /// </summary>
            /// <returns>A guid value</returns>
            public Guid? GetUniqueMunicipalityId()
            {
                return (!string.IsNullOrEmpty(MunicipalityKey)) ? IdFactory.CreateIdFromValue($"{MunicipalityKey}") : null;
            }
        }
    }
}
