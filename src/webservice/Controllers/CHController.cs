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
using OpenPlzApi.DataLayer.CH;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OpenPlzApi.CH
{
    /// <summary>
    /// API controller for Swiss data
    /// </summary>
    [Route("ch")]
    [SwaggerTag("Swiss cantons, districts, communes, localities and streets")]
    public class CHController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CHController"/> class.
        /// </summary>
        /// <param name="dbContext">Injected database context</param>
        public CHController(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Returns all cantons (Kantone).
        /// </summary>
        /// <returns>List of cantons</returns>
        [HttpGet("Cantons")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<CantonResponse>> GetCantonsAsync()
        {
            return await _dbContext.Set<Canton>()
                .OrderBy(x => x.Key)
                .Select(x => new CantonResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all communes (Gemeinden) within a canton (Kanton).
        /// </summary>
        /// <param name="key" example="10">Key of the canton</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>List of communes</returns>
        [HttpGet("Cantons/{key}/Communes")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<CommuneResponse>> GetCommunesByCantonAsync(string key,
            [FromQuery, Range(1, int.MaxValue)] int? page = 1,
            [FromQuery, Range(1, 50)] int? pageSize = 10)
        {
            return await _dbContext.Set<Commune>()
                .Include(x => x.District).ThenInclude(x => x.Canton)
                .Where(x => x.District.Canton.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new CommuneResponse(x))
                .Paging(page ?? 1, pageSize ?? 10)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all communes (Gemeinden) within a district (Bezirk).
        /// </summary>
        /// <param name="key" example="1002">Key of the district</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>List of communes</returns>
        [HttpGet("Districts/{key}/Communes")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<CommuneResponse>> GetCommunesByDistrictAsync(string key,
            [FromQuery, Range(1, int.MaxValue)] int? page = 1,
            [FromQuery, Range(1, 50)] int? pageSize = 10)
        {
            return await _dbContext.Set<Commune>()
                .Include(x => x.District).ThenInclude(x => x.Canton)
                .Where(x => x.District.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new CommuneResponse(x))
                .Paging(page ?? 1, pageSize ?? 10)
                .AsNoTracking()
                .ToListAsync();
        }


        /// <summary>
        /// Returns all districts (Bezirke) within a canton (Kanton).
        /// </summary>
        /// <param name="key" example="10">Key of the canton</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>List of districts</returns>
        [HttpGet("Cantons/{key}/Districts")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<DistrictResponse>> GetDistrictsByCantonAsync(string key,
            [FromQuery, Range(1, int.MaxValue)] int? page = 1,
            [FromQuery, Range(1, 50)] int? pageSize = 10)
        {
            return await _dbContext.Set<District>()
                .Include(x => x.Canton)
                .Where(x => x.Canton.Key == key)
                .OrderBy(x => x.Key)
                .Select(x => new DistrictResponse(x))
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
                    .Include(x => x.Commune).ThenInclude(x => x.District).ThenInclude(x => x.Canton)
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
        /// Returns all localities wihtin a canton (Kanton)
        /// </summary>
        /// <param name="key" example="7">Regional key of the canton</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>Paged list of localities</returns>
        [HttpGet("Cantons/{key}/Localities")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<LocalityResponse>> GetLocalitiesByCantonAsync(string key,
            [FromQuery, Range(1, int.MaxValue)] int? page = 1,
            [FromQuery, Range(1, 50)] int? pageSize = 10)
        {
            return await _dbContext.Set<Locality>()
                .Include(x => x.Commune).ThenInclude(x => x.District).ThenInclude(x => x.Canton)
                .Where(x => x.Commune.District.Canton.Key == key)
                .OrderBy(x => x.PostalCode).ThenBy(x => x.Name)
                .Select(x => new LocalityResponse(x))
                .Paging(page ?? 1, pageSize ?? 10)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all localities wihtin a district (Kreis).
        /// </summary>
        /// <param name="key" example="701">Regional key of the district</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>Paged list of localities</returns>
        [HttpGet("Districts/{key}/Localities")]
        [Produces("text/plain", "text/json", "application/json", "text/csv")]
        public async Task<IEnumerable<LocalityResponse>> GetLocalitiesByDistrictAsync(string key,
            [FromQuery, Range(1, int.MaxValue)] int? page = 1,
            [FromQuery, Range(1, 50)] int? pageSize = 10)
        {
            return await _dbContext.Set<Locality>()
                .Include(x => x.Commune).ThenInclude(x => x.District).ThenInclude(x => x.Canton)
                .Where(x => x.Commune.District.Key == key)
                .OrderBy(x => x.PostalCode).ThenBy(x => x.Name)
                .Select(x => new LocalityResponse(x))
                .Paging(page ?? 1, pageSize ?? 10)
                .AsNoTracking()
                .ToListAsync();
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
                    .Include(x => x.Locality).ThenInclude(x => x.Commune).ThenInclude(x => x.District).ThenInclude(x => x.Canton)
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
