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

using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OpenPlzApi
{
    /// <summary>
    /// Extension methods for <see cref="Enum"/>
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// If <paramref name="value"/> has <see cref="DisplayAttribute"/> defined, this will return 
        /// the name value. Otherwise, null will be returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type</typeparam>
        /// <param name="value">The enum value</param>
        /// <returns>The name value if defined or null</returns>
        public static string GetDisplayName<TEnum>(this TEnum value) 
            where TEnum : Enum
        { 
            return value.GetEnumAttribute<TEnum, DisplayAttribute>()?.Name;
        }

        /// <summary>
        /// If <paramref name="value"/> has <see cref="DisplayAttribute"/> defined, this will return 
        /// the <see cref="DisplayAttribute"/> instance. Otherwise, null will be returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type</typeparam>
        /// <typeparam name="TAttribute">An attribute type</typeparam>
        /// <param name="value">The enum value</param>
        /// <returns><see cref="DisplayAttribute"/> instance if defined or null</returns>
        public static TAttribute GetEnumAttribute<TEnum, TAttribute>(this TEnum value)
            where TEnum : Enum
            where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(value);
            var memberInfo = typeof(TEnum).GetMember(value.ToString());
            return memberInfo[0].GetCustomAttribute<TAttribute>();
        }
    }
}
