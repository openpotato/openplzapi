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
    /// <param name="dbContext">Injected database context</param>
    [Route("li")]
    [SwaggerTag("Liechtenstein communes, localities and streets")]
    public class LIController(AppDbContext dbContext) : BaseController(dbContext)
    {
        /// <summary>
        /// Performs a full-text search using the street name, postal code and city.
        /// </summary>
        /// <param name="searchTerm" example="Alte Landstrasse 9490 Vaduz">Search term for full text search</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Paged list of streets</returns>
        [HttpGet("FullTextSearch")]
        [ProducesResponseType(typeof(IEnumerable<StreetResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<StreetResponse>> FullTextSearchAsync(
            [FromQuery, Required] string searchTerm,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<FullTextStreet>()
                .Include(x => x.Commune)
                .Where(x => x.SearchVector.Matches(EF.Functions.WebSearchToTsQuery("config_openplzapi", searchTerm)))
                .OrderBy(x => x.Name).ThenBy(x => x.PostalCode).ThenBy(x => x.Locality)
                .Select(x => new StreetResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all communes (Gemeinden).
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of communes</returns>
        [HttpGet("Communes")]
        [ProducesResponseType(typeof(IEnumerable<CommuneResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        public async Task<IEnumerable<CommuneResponse>> GetCommunesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Commune>()
                .OrderBy(x => x.Key)
                .Select(x => new CommuneResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all localities whose postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="postalCode">Postal code or regular expression</param>
        /// <param name="name" example="Vaduz">Name or regular expression</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Paged list of localities</returns>
        [HttpGet("Localities")]
        [ProducesResponseType(typeof(IEnumerable<LocalityResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<LocalityResponse>> GetLocalitiesAsync(
            [FromQuery] string postalCode = null,
            [FromQuery] string name = null,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(postalCode))
            {
                return await _dbContext.Set<Locality>()
                    .Include(x => x.Commune)
                    .Where(x => string.IsNullOrEmpty(postalCode) || Regex.IsMatch(x.PostalCode, postalCode))
                    .Where(x => string.IsNullOrEmpty(name) || Regex.IsMatch(x.Name, name, RegexOptions.IgnoreCase))
                    .OrderBy(x => x.PostalCode).ThenBy(x => x.Name)
                    .Select(x => new LocalityResponse(x))
                    .AsNoTracking()
                    .ToPageAsync(page, pageSize);
            }
            else
            {
                throw new BadHttpRequestException("No postal code or name given.");
            }
        }

        /// <summary>
        /// Returns all streets whose name, postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="name" example="Alte Landstrasse">Name or regular expression</param>
        /// <param name="postalCode" example="9490">Postal code or regular expression</param>
        /// <param name="locality" example="Vaduz">Locality or regular expression</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Paged list of streets</returns>
        [HttpGet("Streets")]
        [ProducesResponseType(typeof(IEnumerable<StreetResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<StreetResponse>> GetStreetsAsync(
            [FromQuery] string name = null,
            [FromQuery] string postalCode = null,
            [FromQuery] string locality = null,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(postalCode) || !string.IsNullOrEmpty(locality))
            {
                return await _dbContext.Set<Street>()
                    .Where(x => string.IsNullOrEmpty(name) || Regex.IsMatch(x.Name, name, RegexOptions.IgnoreCase))
                    .Where(x => string.IsNullOrEmpty(postalCode) || Regex.IsMatch(x.Locality.PostalCode, postalCode))
                    .Where(x => string.IsNullOrEmpty(locality) || Regex.IsMatch(x.Locality.Name, locality, RegexOptions.IgnoreCase))
                    .Include(x => x.Locality).ThenInclude(x => x.Commune)
                    .OrderBy(x => x.Name).ThenBy(x => x.Locality.PostalCode).ThenBy(x => x.Locality.Name)
                    .Select(x => new StreetResponse(x))
                    .AsNoTracking()
                    .ToPageAsync(page, pageSize);
            }
            else
            {
                throw new BadHttpRequestException("No name, postal code or locality given.");
            }
        }
    }
}
