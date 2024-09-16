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

using System.Collections.Generic;

namespace OpenPlzApi.CLI
{
    /// <summary>
    /// Raw data sources
    /// </summary>
    public class SourcesConfiguration
    {
        /// <summary>
        /// Raw data sources for Austria
        /// </summary>
        public ATConfiguration AT { get; set; }

        /// <summary>
        /// Raw data sources for Switzerland
        /// </summary>
        public CHConfiguration CH { get; set; }

        /// <summary>
        /// Raw data sources for Germany
        /// </summary>
        public DEConfiguration DE { get; set; }

        /// <summary>
        /// Root folder name for relative paths
        /// </summary>
        public string RootFolderName { get; set; }

        /// <summary>
        /// Raw data sources for Austria
        /// </summary>
        public class ATConfiguration
        {
            public SingleFileConfiguration Districts { get; set; }
            public SingleFileConfiguration Municipalities { get; set; }
            public IList<SingleFileConfiguration> Streets { get; set; } = new List<SingleFileConfiguration>();
        }

        /// <summary>
        /// Raw data sources for Switzerland
        /// </summary>
        public class CHConfiguration
        {
            public SingleFileConfiguration Communes { get; set; }
            public ZipFileConfiguration Streets { get; set; }
        }

        /// <summary>
        /// Raw data sources for Germany
        /// </summary>
        public class DEConfiguration
        {
            public ZipFileConfiguration Municipalities { get; set; }
            public SingleFileConfiguration Streets { get; set; }
        }
    }
}
