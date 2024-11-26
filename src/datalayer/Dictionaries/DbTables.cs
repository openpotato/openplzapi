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

namespace OpenPlzApi.DataLayer
{
    /// <summary>
    /// SQL table names for database
    /// </summary>
    public static class DbTables
    {
        public static class AT
        {
            public const string Districts = "Districts";
            public const string FederalProvince = "FederalProvinces";
            public const string FullTextStreet = "FullTextStreets";
            public const string Locality = "Localities";
            public const string Municipality = "Municipalities";
            public const string Street = "Streets";
        }

        public static class CH
        {
            public const string Canton = "Cantons";
            public const string Commune = "Communes";
            public const string District = "Districts";
            public const string FullTextStreet = "FullTextStreets";
            public const string Locality = "Localities";
            public const string Street = "Streets";
        }

        public static class DE
        {
            public const string District = "Districts";
            public const string FederalState = "FederalStates";
            public const string FullTextStreet = "FullTextStreets";
            public const string GovernmentRegion = "GovernmentRegions";
            public const string Locality = "Localities";
            public const string MunicipalAssociation = "MunicipalAssociations";
            public const string Municipality = "Municipalities";
            public const string Street = "Streets";
        }

        public static class LI
        {
            public const string Commune = "Communes";
            public const string FullTextStreet = "FullTextStreets";
            public const string Locality = "Localities";
            public const string Street = "Streets";
        }
    }
}