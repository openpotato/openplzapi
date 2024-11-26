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

namespace OpenPlzApi
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable{TSource}"/>
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Asynchronously creates a <see cref="Page{TSource}" /> instance from an <see cref="IQueryable{TSource}" /> by first paging 
        /// it and then enumerating it asynchronously.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The data source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Page{TSource}" /> that 
        /// contains elements from the input sequence and corresponding paging information.</returns>
        public static async Task<Page<TSource>> ToPageAsync<TSource>(this IQueryable<TSource> source, 
            int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(source);

            var totalCount = await source.CountAsync(cancellationToken);

            return new Page<TSource>(await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken), pageIndex, pageSize, totalCount);
        }
    }
}
