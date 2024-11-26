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

using System.Collections;

namespace OpenPlzApi
{
    /// <summary>
    /// A page is the result of pagination, in which a potentially large object list is divided 
    /// into smaller units. An implementaion of <see cref="IPage"/> represents one page.
    /// </summary>
    public interface IPage: IEnumerable
    {
        /// <summary>
        /// The index of the page
        /// </summary>
        /// <returns>A number</returns>
        int GetPageIndex();

        /// <summary>
        /// The size of the page
        /// </summary>
        /// <returns>A number</returns>
        int GetPageSize();

        /// <summary>
        /// The total count of elements of the original List
        /// </summary>
        /// <returns>A number</returns>
        int GetTotalCount();

        /// <summary>
        /// The total count of pages for the original List
        /// </summary>
        /// <returns>A number</returns>
        int GetTotalPages();
    }
}
