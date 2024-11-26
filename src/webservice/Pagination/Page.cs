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
    /// Implementation of <see cref="IPage"/>
    /// </summary>
    public class Page<T>: IPage, IEnumerable<T>
    {
        private readonly IList<T> _elements;
        private readonly int _pageIndex;
        private readonly int _pageSize;
        private readonly int _totalCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="Page{T}"/> class.
        /// </summary>
        /// <param name="elements">A subset of the full list</param>
        /// <param name="pageIndex">The index of the page</param>
        /// <param name="pageSize">The size of the page</param>
        /// <param name="totalCount">The total count of pages for the full List</param>
        public Page(IList<T> elements, int pageIndex, int pageSize, int totalCount) 
        { 
            _elements = elements;
            _pageIndex = pageIndex;
            _pageSize = pageSize;
            _totalCount = totalCount;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through 
        /// the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through 
        /// the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        /// <summary>
        /// The index of the page
        /// </summary>
        /// <returns>A number</returns>
        public int GetPageIndex() 
        { 
            return _pageIndex; 
        }

        /// <summary>
        /// The size of the page
        /// </summary>
        /// <returns>A number</returns>
        public int GetPageSize() 
        { 
            return _pageSize; 
        }

        /// <summary>
        /// The total count of elements of the original List
        /// </summary>
        /// <returns>A number</returns>
        public int GetTotalCount() 
        { 
            return _totalCount; 
        }

        /// <summary>
        /// The calculated total count of pages for the original List
        /// </summary>
        /// <returns>A number</returns>
        public int GetTotalPages() 
        { 
            return (_totalCount / _pageSize) + 1; 
        }
    }
}
