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

using System.Reflection;

namespace OpenPlzApi.CLI
{
    /// <summary>
    /// Helper class to extract assambly infos
    /// </summary>
    public static class AssemblyInfo
    {
        public static string GetAgentName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }

        public static string GetTitle()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), true);
            if (attributes.Length > 0)
            {
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
            else
            {
                return null;
            }
        }

        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}