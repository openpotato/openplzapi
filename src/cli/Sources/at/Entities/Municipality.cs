#region OpenPLZ API - Copyright (C) 2022 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2022 STÜBER SYSTEMS GmbH
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
    /// Austrian municipality (Gemeinde)
    /// </summary>
    public class Municipality : BaseRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Municipality"/> class.
        /// </summary>
        /// <param name="timeStamp">A time stamp</param>
        public Municipality(DateOnly timeStamp)
            : base(timeStamp)
        {
        }

        /// <summary>
        /// Array of addtional postal codes
        /// </summary>
        public string[] AdditionalPostalCodes { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Reference to district 
        /// </summary>
        public _District District { get; set; }

        /// <summary>
        /// Postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public MunicipalityStatus Status { get; set; }

        /// <summary>
        /// Get a predictable unique id for this municipality
        /// </summary>
        /// <returns>A guid value</returns>
        public Guid GetUniqueId()
        {
            return IdFactory.CreateIdFromValue(Code);
        }

        /// <summary>
        /// Embedded district (Bezirk)
        /// </summary>
        public class _District
        {
            /// <summary>
            /// Code 
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Get a predictable unique id for this district
            /// </summary>
            /// <returns>A guid value</returns>
            public Guid GetUniqueId()
            {
                return IdFactory.CreateIdFromValue(Code);
            }

        }
    }
}
