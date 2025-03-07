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
using Microsoft.EntityFrameworkCore;
using OpenPlzApi.DataLayer;
using OpenPlzApi.DataLayer.DE;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OpenPlzApi.DE
{
    /// <summary>
    /// API controller for German data
    /// </summary>
    /// <param name="dbContext">Injected database context</param>
    [Route("de")]
    [SwaggerTag("German federal states, government regions, districts, municipalities, municipal associations, localities and streets")]
    public class DEController(AppDbContext dbContext) : BaseController(dbContext)
    {
        /// <summary>
        /// Performs a full-text search using the street name, postal code and city.
        /// </summary>
        /// <param name="searchTerm" example="Berlin, Pariser Platz">Search term for full text search</param>
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
                .Include(x => x.Municipality).ThenInclude(x => x.FederalState)
                .Include(x => x.Municipality).ThenInclude(x => x.District)
                .Where(x => x.SearchVector.Matches(EF.Functions.WebSearchToTsQuery("config_openplzapi", searchTerm)))
                .OrderBy(x => x.Name).ThenBy(x => x.PostalCode).ThenBy(x => x.Locality)
                .Select(x => new StreetResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all districts (Kreise) within a federal state (Bundesland).
        /// </summary>
        /// <param name="key" example="09">Regional key of the federal state</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of districts</returns>
        [HttpGet("FederalStates/{key}/Districts")]
        [ProducesResponseType(typeof(IEnumerable<DistrictResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<DistrictResponse>> GetDistrictsByFederalStateAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<District>()
                .Include(x => x.FederalState)
                .Include(x => x.GovernmentRegion)
                .Where(x => x.FederalState.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new DistrictResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all districts (Kreise) within a government region (Regierungsbezirk).
        /// </summary>
        /// <param name="key" example="091">Regional key of the government region</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of districts</returns>
        [HttpGet("GovernmentRegions/{key}/Districts")]
        [ProducesResponseType(typeof(IEnumerable<DistrictResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<DistrictResponse>> GetDistrictsByGovernmentRegionAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<District>()
                .Include(x => x.FederalState)
                .Include(x => x.GovernmentRegion)
                .Where(x => x.GovernmentRegion.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new DistrictResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all federal states (Bundesländer).
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of federal states</returns>
        [HttpGet("FederalStates")]
        [ProducesResponseType(typeof(IEnumerable<FederalStateResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        public async Task<IEnumerable<FederalStateResponse>> GetFederalStatesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<FederalState>()
                .OrderBy(x => x.Key)
                .Select(x => new FederalStateResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all government regions (Regierungsbezirke) within a federal state (Bundesaland).
        /// </summary>
        /// <param name="key" example="09">Regional key of the federal state</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of government regions</returns>
        [HttpGet("FederalStates/{key}/GovernmentRegions")]
        [ProducesResponseType(typeof(IEnumerable<GovernmentRegionResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<GovernmentRegionResponse>> GetGovernmentRegionsByFederalStateAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<GovernmentRegion>()
                .Include(x => x.FederalState)
                .Where(x => x.FederalState.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new GovernmentRegionResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all localities whose postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="postalCode" example="56566">Postal code or regular expression</param>
        /// <param name="name">Name or regular expression</param>
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
                    .Include(x => x.Municipality).ThenInclude(x => x.FederalState)
                    .Include(x => x.Municipality).ThenInclude(x => x.District)
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
        /// <param name="key" example="09180">Regional key of the district</param>
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
                .Include(x => x.Municipality).ThenInclude(x => x.FederalState)
                .Include(x => x.Municipality).ThenInclude(x => x.District)
                .Where(x => x.Municipality.District.Key == key)
                .OrderBy(x => x.PostalCode).ThenBy(x => x.Name)
                .Select(x => new LocalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all localities wihtin a federal state (Bundesland).
        /// </summary>
        /// <param name="key" example="11">Regional key of the federal state</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Paged list of localities</returns>
        [HttpGet("FederalStates/{key}/Localities")]
        [ProducesResponseType(typeof(IEnumerable<LocalityResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<LocalityResponse>> GetLocalitiesByFederalStateAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Locality>()
                .Include(x => x.Municipality).ThenInclude(x => x.FederalState)
                .Include(x => x.Municipality).ThenInclude(x => x.District)
                .Where(x => x.Municipality.FederalState.Key == key)
                .OrderBy(x => x.PostalCode).ThenBy(x => x.Name)
                .Select(x => new LocalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all localities wihtin a government region (Regierungsbezirk).
        /// </summary>
        /// <param name="key" example="091">Regional key of the government region</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Paged list of localities</returns>
        [HttpGet("GovernmentRegions/{key}/Localities")]
        [ProducesResponseType(typeof(IEnumerable<LocalityResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<LocalityResponse>> GetLocalitiesByGovernmentRegionAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Locality>()
                .Include(x => x.Municipality).ThenInclude(x => x.FederalState)
                .Include(x => x.Municipality).ThenInclude(x => x.District)
                .Where(x => x.Municipality.District.GovernmentRegion.Key == key)
                .OrderBy(x => x.PostalCode).ThenBy(x => x.Name)
                .Select(x => new LocalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all municipal associations (Gemeindeverbände) within a district (Kreis).
        /// </summary>
        /// <param name="key" example="09180">Regional key of the district</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of municipal associations</returns>
        [HttpGet("Districts/{key}/MunicipalAssociations")]
        [ProducesResponseType(typeof(IEnumerable<MunicipalAssociationResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<MunicipalAssociationResponse>> GetMunicipalAssociationsByDistrictAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<MunicipalAssociation>()
                .Include(x => x.District).ThenInclude(x => x.FederalState)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new MunicipalAssociationResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all municipal associations (Gemeindeverbände) within a federal state (Bundesland).
        /// </summary>
        /// <param name="key" example="09">Regional key of the federal state</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of municipal associations</returns>
        [HttpGet("FederalStates/{key}/MunicipalAssociations")]
        [ProducesResponseType(typeof(IEnumerable<MunicipalAssociationResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<MunicipalAssociationResponse>> GetMunicipalAssociationsByFederalStateAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<MunicipalAssociation>()
                .Include(x => x.District).ThenInclude(x => x.FederalState)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.FederalState.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new MunicipalAssociationResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all municipal associations (Gemeindeverbünde) within a government region (Regierungsbezirk).
        /// </summary>
        /// <param name="key" example="091">Regional key of the government region</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of municipal associations</returns>
        [HttpGet("GovernmentRegions/{key}/MunicipalAssociations")]
        [ProducesResponseType(typeof(IEnumerable<MunicipalAssociationResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<MunicipalAssociationResponse>> GetMunicipalAssociationsByGovernmentRegionAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<MunicipalAssociation>()
                .Include(x => x.District).ThenInclude(x => x.FederalState)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.GovernmentRegion.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new MunicipalAssociationResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all municipalities (Gemeinden) within a district (Kreis).
        /// </summary>
        /// <param name="key" example="09180">Regional key of the district</param>
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
                .Include(x => x.FederalState)
                .Include(x => x.Association)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new MunicipalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all municipalities (Gemeinden) within a federal state (Bundesland).
        /// </summary>
        /// <param name="key" example="09">Regional key of the federal state</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of municipalities</returns>
        [HttpGet("FederalStates/{key}/Municipalities")]
        [ProducesResponseType(typeof(IEnumerable<MunicipalityResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<MunicipalityResponse>> GetMunicipalitiesByFederalStateAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Municipality>()
                .Include(x => x.FederalState)
                .Include(x => x.Association)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.FederalState.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new MunicipalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all municipalities (Gemeinden) within a government region (Regierungsbezirk).
        /// </summary>
        /// <param name="key" example="091">Regional key of the government region</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>List of municipalities</returns>
        [HttpGet("GovernmentRegions/{key}/Municipalities")]
        [ProducesResponseType(typeof(IEnumerable<MunicipalityResponse>), statusCode: 200, MediaTypeNames.Application.Json, MediaTypeNames.Text.Json, MediaTypeNames.Text.Plain, MediaTypeNames.Text.Csv)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 400, MediaTypeNames.Application.ProblemDetails)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode: 500, MediaTypeNames.Application.ProblemDetails)]
        [PaginationFilter]
        public async Task<IEnumerable<MunicipalityResponse>> GetMunicipalitiesByGovernmentRegionAsync(
            [FromRoute] string key,
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Municipality>()
                .Include(x => x.FederalState)
                .Include(x => x.Association)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.GovernmentRegion.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new MunicipalityResponse(x))
                .AsNoTracking()
                .ToPageAsync(page, pageSize);
        }

        /// <summary>
        /// Returns all streets whose name, postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="name" example="Pariser Platz">Name or regular expression</param>
        /// <param name="postalCode">Postal code or regular expression</param>
        /// <param name="locality" example="Berlin">Locality or regular expression</param>
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
                    .Include(x => x.Locality).ThenInclude(x => x.Municipality).ThenInclude(x => x.FederalState)
                    .Include(x => x.Locality).ThenInclude(x => x.Municipality).ThenInclude(x => x.District)
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
