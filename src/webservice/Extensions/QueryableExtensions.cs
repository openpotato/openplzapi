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

namespace OpenPlzApi
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable{T}"/>
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Combines Skip() and the Take() commnad"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the data source</typeparam>
        /// <param name="source">The data source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged data source</returns>
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, int pageIndex, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(source);

            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}
