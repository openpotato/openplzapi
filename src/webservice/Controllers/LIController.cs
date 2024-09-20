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
using Microsoft.EntityFrameworkCore;
using OpenPlzApi.DataLayer;
using OpenPlzApi.DataLayer.LI;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OpenPlzApi.LI
{
    /// <summary>
    /// API controller for Liechtenstein data
    /// </summary>
    [Route("li")]
    [SwaggerTag("Liechtenstein communes, localities and streets")]
    public class LIController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LIController"/> class.
        /// </summary>
        /// <param name="dbContext">Injected database context</param>
        public LIController(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Returns all communes (Gemeinden).
        /// </summary>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>List of communes</returns>
        [HttpGet("Cantons/Communes")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<CommuneResponse>> GetCommunesAsync(
            [FromQuery, Range(1, int.MaxValue)] int? page = 1,
            [FromQuery, Range(1, 50)] int? pageSize = 10)
        {
            return await _dbContext.Set<Commune>()
                .OrderBy(x => x.Key)
                .Select(x => new CommuneResponse(x))
                .Paging(page ?? 1, pageSize ?? 10)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all localities whose postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="postalCode">Postal code or regular expression</param>
        /// <param name="name" example="Zürich">Name or regular expression</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>Paged list of localities</returns>
        [HttpGet("Localities")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<LocalityResponse>> GetLocalitiesAsync(
            [FromQuery] string postalCode,
            [FromQuery] string name,
            [FromQuery, Range(1, int.MaxValue)] int? page = 1,
            [FromQuery, Range(1, 50)] int? pageSize = 10)
        {
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(postalCode))
            {
                return await _dbContext.Set<Locality>()
                    .Include(x => x.Commune)
                    .Where(x => string.IsNullOrEmpty(postalCode) || Regex.IsMatch(x.PostalCode, postalCode))
                    .Where(x => string.IsNullOrEmpty(name) || Regex.IsMatch(x.Name, name, RegexOptions.IgnoreCase))
                    .OrderBy(x => x.PostalCode).ThenBy(x => x.Name)
                    .Select(x => new LocalityResponse(x))
                    .Paging(page ?? 1, pageSize ?? 10)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                throw new ArgumentNullException(nameof(name));
            }
        }

        /// <summary>
        /// Returns all streets whose name, postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="name" example="Bederstrasse">Name or regular expression</param>
        /// <param name="postalCode" example="8002">Postal code or regular expression</param>
        /// <param name="locality">Locality or regular expression</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>Paged list of streets</returns>
        [HttpGet("Streets")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<StreetResponse>> GetStreetsAsync(
            [FromQuery] string name, 
            [FromQuery] string postalCode, 
            [FromQuery] string locality,
            [FromQuery, Range(1, int.MaxValue)] int? page = 1,
            [FromQuery, Range(1, 50)] int? pageSize = 10)
        {
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(postalCode) || !string.IsNullOrEmpty(locality))
            {
                return await _dbContext.Set<Street>()
                    .Include(x => x.Locality).ThenInclude(x => x.Commune)
                    .Where(x => string.IsNullOrEmpty(name) || Regex.IsMatch(x.Name, name, RegexOptions.IgnoreCase))
                    .Where(x => string.IsNullOrEmpty(postalCode) || Regex.IsMatch(x.Locality.PostalCode, postalCode))
                    .Where(x => string.IsNullOrEmpty(locality) || Regex.IsMatch(x.Locality.Name, locality, RegexOptions.IgnoreCase))
                    .OrderBy(x => x.Name).ThenBy(x => x.Locality.PostalCode).ThenBy(x => x.Locality.Name)
                    .Paging(page ?? 1, pageSize ?? 10)
                    .Select(x => new StreetResponse(x))
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                throw new ArgumentNullException(nameof(name));
            }
        }
    }
}
