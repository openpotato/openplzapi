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
using OpenPlzApi.DataLayer.AT;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OpenPlzApi.AT
{
    /// <summary>
    /// API controller for Austrian data
    /// </summary>
    /// <param name="dbContext">Injected database context</param>
    [Route("at")]
    [SwaggerTag("Austrian federal provinces, districts, municipalities, localities and streets")]
    public class ATController(AppDbContext dbContext) : BaseController(dbContext)
    {
        /// <summary>
        /// Performs a full-text search using the street name, postal code and city.
        /// </summary>
        /// <param name="searchTerm" example="1020 Wien Adambergergasse">Search term for full text search</param>
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
                .Include(x => x.Municipality).ThenInclude(x => x.District).ThenInclude(x => x.FederalProvince)
                .Where(x => x.SearchVector.Matches(EF.Functions.WebSearchToTsQuery("config_openplzapi", searchTerm)))
                .OrderBy(x => x.Name).ThenBy(x => x.PostalCode).ThenBy(x => x.Locality)
                .Select(x => new StreetResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all districts (Bezirke) within a federal province (Bundesland).
        /// </summary>
        /// <param name="key" example="7">Key of the federal province</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of districts</returns>
        [HttpGet("FederalProvinces/{key}/Districts")]
        [ProducesResponseType(typeof(IEnumerable<DistrictResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<DistrictResponse>> GetDistrictsByFederalProvinceAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<District>()
                .Include(x => x.FederalProvince)
                .Where(x => x.FederalProvince.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new DistrictResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all federal provinces (Bundesländer).
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of federal provinces</returns>
        [HttpGet("FederalProvinces")]
        [ProducesResponseType(typeof(IEnumerable<FederalProvinceResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        public async Task<IEnumerable<FederalProvinceResponse>> GetFederalProvincesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<FederalProvince>()
                .OrderBy(x => x.Key)
                .Select(x => new FederalProvinceResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all localities whose postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="postalCode">Postal code or regular expression</param>
        /// <param name="name" example="Wien">Name or regular expression</param>
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
                    .Include(x => x.Municipality).ThenInclude(x => x.District).ThenInclude(x => x.FederalProvince)
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
        /// Returns all localities wihtin a district (Kreis).
        /// </summary>
        /// <param name="key" example="701">Regional key of the district</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Paged list of localities</returns>
        [HttpGet("Districts/{key}/Localities")]
        [ProducesResponseType(typeof(IEnumerable<LocalityResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<LocalityResponse>> GetLocalitiesByDistrictAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Locality>()
                .Include(x => x.Municipality).ThenInclude(x => x.District).ThenInclude(x => x.FederalProvince)
                .Where(x => x.Municipality.District.Key == key || x.Municipality.District.Code == key)
                .OrderBy(x => x.PostalCode).ThenBy(x => x.Name)
                .Select(x => new LocalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all localities wihtin a federal province (Bundesland).
        /// </summary>
        /// <param name="key" example="7">Regional key of the federal province</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Paged list of localities</returns>
        [HttpGet("FederalProvinces/{key}/Localities")]
        [ProducesResponseType(typeof(IEnumerable<LocalityResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<LocalityResponse>> GetLocalitiesByFederalProvinceAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Locality>()
                .Include(x => x.Municipality).ThenInclude(x => x.District).ThenInclude(x => x.FederalProvince)
                .Where(x => x.Municipality.District.FederalProvince.Key == key)
                .OrderBy(x => x.PostalCode).ThenBy(x => x.Name)
                .Select(x => new LocalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all municipalities (Gemeinden) within a district (Bezirk).
        /// </summary>
        /// <param name="key" example="701">Key of the district</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of municipalities</returns>
        [HttpGet("Districts/{key}/Municipalities")]
        [ProducesResponseType(typeof(IEnumerable<MunicipalityResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<MunicipalityResponse>> GetMunicipalitiesByDistrictAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Municipality>()
                .Include(x => x.District).ThenInclude(x => x.FederalProvince)
                .Where(x => x.District.Key == key || x.District.Code == key)
                .OrderBy(x => x.Key)
                .Select(x => new MunicipalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all municipalities (Gemeinden) within a federal province (Bundesland).
        /// </summary>
        /// <param name="key" example="7">Key of the federal province</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of municipalities</returns>
        [HttpGet("FederalProvinces/{key}/Municipalities")]
        [ProducesResponseType(typeof(IEnumerable<MunicipalityResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<MunicipalityResponse>> GetMunicipalitiesByFederalProvinceAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Municipality>()
                .Include(x => x.District).ThenInclude(x => x.FederalProvince)
                .Where(x => x.District.FederalProvince.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new MunicipalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all streets whose name, postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="name">Name or regular expression</param>
        /// <param name="postalCode" example="1020">Postal code or regular expression</param>
        /// <param name="locality">Locality or regular expression</param>
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
                    .Include(x => x.Locality).ThenInclude(x => x.Municipality).ThenInclude(x => x.District).ThenInclude(x => x.FederalProvince)
                    .Where(x => string.IsNullOrEmpty(name) || Regex.IsMatch(x.Name, name, RegexOptions.IgnoreCase))
                    .Where(x => string.IsNullOrEmpty(postalCode) || Regex.IsMatch(x.Locality.PostalCode, postalCode))
                    .Where(x => string.IsNullOrEmpty(locality) || Regex.IsMatch(x.Locality.Name, locality, RegexOptions.IgnoreCase))
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