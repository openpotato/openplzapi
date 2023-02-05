#region OpenPLZ API - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    OpenPLZ API 
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
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
    [Route("de")]
    [SwaggerTag("German federal states, government regions, districts, municipalities, municipal associations, localities and streets")]
    public class DEController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DEController"/> class.
        /// </summary>
        /// <param name="dbContext">Injected database context</param>
        public DEController(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Returns all districts (Kreise) within a federal state (Bundesland).
        /// </summary>
        /// <param name="key" example="09">Regional key of the federal state</param>
        /// <returns>List of districts</returns>
        [HttpGet("FederalStates/{key}/Districts")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<DistrictResponse>> GetDistrictsByFederalStateAsync(string key)
        {
            return await _dbContext.Set<District>()
                .Include(x => x.FederalState)
                .Include(x => x.GovernmentRegion)
                .Where(x => x.FederalState.RegionalKey == key)
                .OrderBy(x => x.RegionalKey)
                .Select(x => new DistrictResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all districts (Kreise) within a government region (Regierungsbezirk).
        /// </summary>
        /// <param name="key" example="091">Regional key of the government region</param>
        /// <returns>List of districts</returns>
        [HttpGet("GovernmentRegions/{key}/Districts")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<DistrictResponse>> GetDistrictsByGovernmentRegionAsync(string key)
        {
            return await _dbContext.Set<District>()
                .Include(x => x.FederalState)
                .Include(x => x.GovernmentRegion)
                .Where(x => x.GovernmentRegion.RegionalKey == key)
                .OrderBy(x => x.RegionalKey)
                .Select(x => new DistrictResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all federal states (Bundesländer).
        /// </summary>
        /// <returns>List of federal states</returns>
        [HttpGet("FederalStates")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<FederalStateResponse>> GetFederalStatesAsync()
        {
            return await _dbContext.Set<FederalState>()
                .OrderBy(x => x.RegionalKey)
                .Select(x => new FederalStateResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all government regions (Regierungsbezirke) within a federal state (Bundesaland).
        /// </summary>
        /// <param name="key" example="09">Regional key of the federal state</param>
        /// <returns>List of government regions</returns>
        [HttpGet("FederalStates/{key}/GovernmentRegions")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<GovernmentRegionResponse>> GetGovernmentRegionsByFederalStateAsync(string key)
        {
            return await _dbContext.Set<GovernmentRegion>()
                .Include(x => x.FederalState)
                .Where(x => x.FederalState.RegionalKey == key)
                .OrderBy(x => x.RegionalKey)
                .Select(x => new GovernmentRegionResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all localities whose postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="postalCode" example="56566">Postal code or regular expression</param>
        /// <param name="name">Name or regular expression</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>Paged list of localities</returns>
        [HttpGet("Localities")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<LocalityResponse>> GetLocalitiesAsync(
            [FromQuery] string postalCode, 
            [FromQuery] string name,
            [FromQuery, Range(1, int.MaxValue)] int? page = 1,
            [FromQuery, Range(1, 50)] int? pageSize = 10)
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
        /// Returns all municipal associations (Gemeindeverbände) within a district (Kreis).
        /// </summary>
        /// <param name="key" example="09180">Regional key of the district</param>
        /// <returns>List of municipal associations</returns>
        [HttpGet("Districts/{key}/MunicipalAssociations")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<MunicipalAssociationResponse>> GetMunicipalAssociationsByDistrictAsync(string key)
        {
            return await _dbContext.Set<MunicipalAssociation>()
                .Include(x => x.District).ThenInclude(x => x.FederalState)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.RegionalKey == key)
                .OrderBy(x => x.RegionalKey)
                .Select(x => new MunicipalAssociationResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all municipal associations (Gemeindeverbände) within a federal state (Bundesland).
        /// </summary>
        /// <param name="key" example="09">Regional key of the federal state</param>
        /// <returns>List of municipal associations</returns>
        [HttpGet("FederalStates/{key}/MunicipalAssociations")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<MunicipalAssociationResponse>> GetMunicipalAssociationsByFederalStateAsync(string key)
        {
            return await _dbContext.Set<MunicipalAssociation>()
                .Include(x => x.District).ThenInclude(x => x.FederalState)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.FederalState.RegionalKey == key)
                .OrderBy(x => x.RegionalKey)
                .Select(x => new MunicipalAssociationResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all municipal associations (Gemeindeverbünde) within a government region (Regierungsbezirk).
        /// </summary>
        /// <param name="key" example="091">Regional key of the government region</param>
        /// <returns>List of municipal associations</returns>
        [HttpGet("GovernmentRegions/{key}/MunicipalAssociations")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<MunicipalAssociationResponse>> GetMunicipalAssociationsByGovernmentRegionAsync(string key)
        {
            return await _dbContext.Set<MunicipalAssociation>()
                .Include(x => x.District).ThenInclude(x => x.FederalState)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.GovernmentRegion.RegionalKey == key)
                .OrderBy(x => x.RegionalKey)
                .Select(x => new MunicipalAssociationResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all municipalities (Gemeinden) within a district (Kreis).
        /// </summary>
        /// <param name="key" example="09180">Regional key of the district</param>
        /// <returns>List of municipalities</returns>
        [HttpGet("Districts/{key}/Municipalities")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<MunicipalityResponse>> GetMunicipalitiesByDistrictAsync(string key)
        {
            return await _dbContext.Set<Municipality>()
                .Include(x => x.FederalState)
                .Include(x => x.Association)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.RegionalKey == key)
                .OrderBy(x => x.RegionalKey)
                .Select(x => new MunicipalityResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all municipalities (Gemeinden) within a federal state (Bundesland).
        /// </summary>
        /// <param name="key" example="Pariser Platz">Regional key of the federal state</param>
        /// <returns>List of municipalities</returns>
        [HttpGet("FederalStates/{key}/Municipalities")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<MunicipalityResponse>> GetMunicipalitiesByFederalStateAsync(string key)
        {
            return await _dbContext.Set<Municipality>()
                .Include(x => x.FederalState)
                .Include(x => x.Association)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.FederalState.RegionalKey == key)
                .OrderBy(x => x.RegionalKey)
                .Select(x => new MunicipalityResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns all municipalities (Gemeinden) within a government region (Regierungsbezirk).
        /// </summary>
        /// <param name="key" example="091">Regional key of the government region</param>
        /// <returns>List of municipalities</returns>
        [HttpGet("GovernmentRegions/{key}/Municipalities")]
        [Produces("text/plain", "text/json", "application/json")]
        public async Task<IEnumerable<MunicipalityResponse>> GetMunicipalitiesByGovernmentRegionAsync(string key)
        {
            return await _dbContext.Set<Municipality>()
                .Include(x => x.FederalState)
                .Include(x => x.Association)
                .Include(x => x.District).ThenInclude(x => x.GovernmentRegion)
                .Where(x => x.District.GovernmentRegion.RegionalKey == key)
                .OrderBy(x => x.RegionalKey)
                .Select(x => new MunicipalityResponse(x))
                .AsNoTracking()
                .ToListAsync();
        }
        /// <summary>
        /// Returns all streets whose name, postal code and/or name matches the given patterns.
        /// </summary>
        /// <param name="name" example="Pariser Platz">Name or regular expression</param>
        /// <param name="postalCode">Postal code or regular expression</param>
        /// <param name="locality" example="Berlin">Locality or regular expression</param>
        /// <param name="page">Page number (starting with 1)</param>
        /// <param name="pageSize">Page size (maximum 50)</param>
        /// <returns>Paged list of streets</returns>
        [HttpGet("Streets")]
        [Produces("text/plain", "text/json", "application/json")]
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
                    .Include(x => x.Locality).ThenInclude(x => x.Municipality).ThenInclude(x => x.FederalState)
                    .Include(x => x.Locality).ThenInclude(x => x.Municipality).ThenInclude(x => x.District)
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
