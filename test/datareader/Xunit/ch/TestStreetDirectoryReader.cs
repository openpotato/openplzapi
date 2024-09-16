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

using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OpenPlzApi.CLI.Sources.CH.Tests
{
    /// <summary>
    /// Unit tests for <see cref="StreetDirectoryReader"/>.
    /// </summary>
    public class TestStreetDirectoryReader
    {
        [Fact]
        public async Task TestStreetParsing()
        {
            using var csvStream = File.OpenText(Path.Combine(GetAssetsFolder(), "streets.csv"));

            var sdReader = new StreetDirectoryReader(csvStream);

            IAsyncEnumerator<Street> enumerator = sdReader.ReadAsync().GetAsyncEnumerator();

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal("1630", enumerator.Current.Localities[0].Commune.Key);
            Assert.Equal("Glarus Nord", enumerator.Current.Localities[0].Commune.Name);
            Assert.Equal("GL", enumerator.Current.Localities[0].Commune.Canton);
            Assert.Equal("Näfels", enumerator.Current.Localities[0].Name);
            Assert.Equal("8752", enumerator.Current.Localities[0].PostalCode);
            Assert.Equal("10061643", enumerator.Current.Key);
            Assert.Equal("Aeschen", enumerator.Current.Name);
            Assert.Equal(StreetType.Street, enumerator.Current.Type);
            Assert.Equal(StreetStatus.Existing, enumerator.Current.Status);
            Assert.Equal(new DateOnly(2021, 4, 7), enumerator.Current.LastModified);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal("1630", enumerator.Current.Localities[0].Commune.Key);
            Assert.Equal("Glarus Nord", enumerator.Current.Localities[0].Commune.Name);
            Assert.Equal("GL", enumerator.Current.Localities[0].Commune.Canton);
            Assert.Equal("Oberurnen", enumerator.Current.Localities[0].Name);
            Assert.Equal("8868", enumerator.Current.Localities[0].PostalCode);
            Assert.Equal("10238439", enumerator.Current.Key);
            Assert.Equal("Panoramastrasse", enumerator.Current.Name);
            Assert.Equal(StreetType.Street, enumerator.Current.Type);
            Assert.Equal(StreetStatus.Existing, enumerator.Current.Status);
            Assert.Equal(new DateOnly(2021, 4, 7), enumerator.Current.LastModified);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal("5514", enumerator.Current.Localities[0].Commune.Key);
            Assert.Equal("Bottens", enumerator.Current.Localities[0].Commune.Name);
            Assert.Equal("VD", enumerator.Current.Localities[0].Commune.Canton);
            Assert.Equal("Bottens", enumerator.Current.Localities[0].Name);
            Assert.Equal("1041", enumerator.Current.Localities[0].PostalCode);
            Assert.Equal("10090616", enumerator.Current.Key);
            Assert.Equal("Les Etramaz", enumerator.Current.Name);
            Assert.Equal(StreetType.Area, enumerator.Current.Type);
            Assert.Equal(StreetStatus.Existing, enumerator.Current.Status);
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.LastModified);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal("605", enumerator.Current.Localities[0].Commune.Key);
            Assert.Equal("Bowil", enumerator.Current.Localities[0].Commune.Name);
            Assert.Equal("BE", enumerator.Current.Localities[0].Commune.Canton);
            Assert.Equal("Bowil", enumerator.Current.Localities[0].Name);
            Assert.Equal("3533", enumerator.Current.Localities[0].PostalCode);
            Assert.Equal("10112650", enumerator.Current.Key);
            Assert.Equal("Gerbe", enumerator.Current.Name);
            Assert.Equal(StreetType.Area, enumerator.Current.Type);
            Assert.Equal(StreetStatus.Existing, enumerator.Current.Status);
            Assert.Equal(new DateOnly(2021, 4, 7), enumerator.Current.LastModified);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal("2236", enumerator.Current.Localities[0].Commune.Key);
            Assert.Equal("Gibloux", enumerator.Current.Localities[0].Commune.Name);
            Assert.Equal("FR", enumerator.Current.Localities[0].Commune.Canton);
            Assert.Equal("Rueyres-St-Laurent", enumerator.Current.Localities[0].Name);
            Assert.Equal("1695", enumerator.Current.Localities[0].PostalCode);
            Assert.Equal("2236", enumerator.Current.Localities[1].Commune.Key);
            Assert.Equal("Gibloux", enumerator.Current.Localities[1].Commune.Name);
            Assert.Equal("FR", enumerator.Current.Localities[1].Commune.Canton);
            Assert.Equal("Villarsel-le-Gibloux", enumerator.Current.Localities[1].Name);
            Assert.Equal("1695", enumerator.Current.Localities[1].PostalCode);
            Assert.Equal("10163894", enumerator.Current.Key);
            Assert.Equal("Route du Glèbe", enumerator.Current.Name);
            Assert.Equal(StreetType.Street, enumerator.Current.Type);
            Assert.Equal(StreetStatus.Existing, enumerator.Current.Status);
            Assert.Equal(new DateOnly(2022, 8, 25), enumerator.Current.LastModified);

            Assert.False(await enumerator.MoveNextAsync());
        }

        private static string GetAssetsFolder()
        {
            // Get the full location of the assembly
            string assemblyPath = System.Reflection.Assembly.GetAssembly(typeof(TestCommuneRegister)).Location;

            // Get the folder that's in
            return Path.Combine(Path.GetDirectoryName(assemblyPath), "ch",  "Assets");
        }
    }
}
