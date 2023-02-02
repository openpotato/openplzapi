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

namespace OpenPlzApi.CLI.Sources.AT
{
    /// <summary>
    /// Austrian district (Bezirk)
    /// </summary>
    public class District : BaseRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="District"/> class.
        /// </summary>
        /// <param name="timeStamp">A time stamp</param>
        public District(DateOnly timeStamp)
            : base(timeStamp)
        {
        }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Reference to federal province
        /// </summary>
        public _FederalProvince FederalProvince { get; set; }

        /// <summary>
        /// Get a predictable unique id for this district
        /// </summary>
        /// <returns>A guid value</returns>
        public Guid GetUniqueId()
        {
            return IdFactory.CreateIdFromValue(Code);
        }

        /// <summary>
        /// Embedded federal province (Bundesland)
        /// </summary>
        public class _FederalProvince
        {
            /// <summary>
            /// Key
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Code
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Get a predictable unique id for this federal province
            /// </summary>
            /// <returns>A guid value</returns>
            public Guid GetUniqueId()
            {
                return IdFactory.CreateIdFromValue(Key);
            }
        }
    }
}
