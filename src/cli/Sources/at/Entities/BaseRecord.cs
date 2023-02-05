﻿#region OpenPLZ API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
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
    /// Base class for <see cref="District"/>, <see cref="Municipality"/> and <see cref="Street"/>
    /// </summary>
    public abstract class BaseRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRecord"/> class.
        /// </summary>
        /// <param name="timeStamp">A time stamp</param>
        public BaseRecord(DateOnly timeStamp)
        {
            TimeStamp = timeStamp;
        }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Time stamp
        /// </summary>
        public DateOnly TimeStamp { get; }
    }
}
