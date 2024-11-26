﻿#region OpenPLZ API - Copyright (c) STÜBER SYSTEMS GmbH
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
using OpenPlzApi.DataLayer;

namespace OpenPlzApi
{
    /// <summary>
    /// Abstract base API controller
    /// </summary>
    /// <param name="dbContext">Injected database context</param>
    [ApiController]
    public abstract class BaseController(AppDbContext dbContext) : ControllerBase
    {
        /// <summary>
        /// Injected database context
        /// </summary>
        protected readonly AppDbContext _dbContext = dbContext;
    }
}
