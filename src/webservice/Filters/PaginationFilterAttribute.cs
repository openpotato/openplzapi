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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OpenPlzApi
{
    /// <summary>
    /// A filter that injects pagination information from a <see cref="IPage" /> controller 
    /// action result as http headers into the response.
    /// </summary>
    public class PaginationFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called after the action method of a controller executes.
        /// </summary>
        /// <param name="context">The context for the action filter</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value is IPage page)
                {
                    context.HttpContext.Response.Headers.Append("x-page", page.GetPageIndex().ToString());
                    context.HttpContext.Response.Headers.Append("x-page-size", page.GetPageSize().ToString());
                    context.HttpContext.Response.Headers.Append("x-total-pages", page.GetTotalPages().ToString());
                    context.HttpContext.Response.Headers.Append("x-total-count", page.GetTotalCount().ToString());
                }
            }
        }
    }
}
 