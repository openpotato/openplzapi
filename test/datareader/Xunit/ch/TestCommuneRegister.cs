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

using System;
using System.IO;
using Xunit;

namespace OpenPlzApi.CLI.Sources.CH.Tests
{
    /// <summary>
    /// Unit tests for <see cref="CommuneRegister"/>.
    /// </summary>
    public class TestCommuneRegister
    {
        [Fact]
        public void TestCantonParsing()
        {
            using var csvStream = File.Open(Path.Combine(GetAssetsFolder(), "communes.xlsx"), FileMode.Open);

            var communeRegister = new CommuneRegister();

            communeRegister.Load(csvStream);

            Assert.Equal(new DateOnly(2022, 1, 1), communeRegister.TimeStamp);
            Assert.Equal("1", communeRegister.Cantons[0].Key);
            Assert.Equal("ZH", communeRegister.Cantons[0].Code);
            Assert.Equal("Zürich", communeRegister.Cantons[0].Name);
        }

        [Fact]
        public void TestCommuneParsing()
        {
            using var csvStream = File.Open(Path.Combine(GetAssetsFolder(), "communes.xlsx"), FileMode.Open);

            var communeRegister = new CommuneRegister();

            communeRegister.Load(csvStream);

            Assert.Equal(new DateOnly(2022, 1, 1), communeRegister.TimeStamp);
            Assert.Equal("1", communeRegister.Communes[0].Key);
            Assert.Equal("Aeugst am Albis", communeRegister.Communes[0].Name);
            Assert.Equal("Aeugst am Albis", communeRegister.Communes[0].ShortName);
            Assert.Equal("Bezirk Affoltern", communeRegister.Communes[0].District.Name);
            Assert.Equal("Zürich", communeRegister.Communes[0].Canton.Name);
        }

        [Fact]
        public void TestDistrictParsing()
        {
            using var csvStream = File.Open(Path.Combine(GetAssetsFolder(), "communes.xlsx"), FileMode.Open);

            var communeRegister = new CommuneRegister();

            communeRegister.Load(csvStream);

            Assert.Equal(new DateOnly(2022, 1, 1), communeRegister.TimeStamp);
            Assert.Equal("101", communeRegister.Districts[0].Key);
            Assert.Equal("Bezirk Affoltern", communeRegister.Districts[0].Name);
            Assert.Equal("Zürich", communeRegister.Districts[0].Canton.Name);
        }

        private static string GetAssetsFolder()
        {
            // Get the full location of the assembly
            string assemblyPath = System.Reflection.Assembly.GetAssembly(typeof(TestCommuneRegister)).Location;

            // Get the folder that's in
            return Path.Combine(Path.GetDirectoryName(assemblyPath), "ch", "Assets");
        }
    }
}
