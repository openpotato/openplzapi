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

using System.CommandLine;

namespace OpenPlzApi.CLI
{
    public static class CommandDefinitions
    {
        public static Command ImportDb(AppConfiguration appConfiguration)
        {
            var command = new Command("importdb", "Imports public data to the OpenPLZ API database")
            {
                new Option<ImportSource>(new[] { "--source", "-s" }, "Name of data source")
                {
                    IsRequired = true
                }
            };

            command.SetHandler(async (ImportSource source)
                => await CommandHandlers.ImportDb(appConfiguration, source),
                    command.Options[0] as Option<ImportSource>);

            return command;
        }

        public static Command InitDb(AppConfiguration appConfiguration)
        {
            var command = new Command("initdb", "Creates or migrates an OpenPLZ API database")
            {
                new Option<bool>(new[] { "--import", "-i" }, "Imports public data")
                {
                    IsRequired = false
                }
            };

            command.SetHandler(async (bool import)
                => await CommandHandlers.InitDb(appConfiguration, import),
                    command.Options[0] as Option<bool>);

            return command;
        }
    }
}