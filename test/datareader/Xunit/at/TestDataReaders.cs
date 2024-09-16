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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OpenPlzApi.CLI.Sources.AT.Tests
{
    /// <summary>
    /// Unit tests for <see cref="StreetDataReader"/>.
    /// </summary>
    public class TestDataReaders
    {
        [Fact]
        public async Task TestDistrictParsing()
        {
            using var csvStream = File.OpenText(Path.Combine(GetAssetsFolder(), "districts.csv"));

            var rdReader = new DistrictDataReader(csvStream);

            IAsyncEnumerator<District> enumerator = rdReader.ReadAsync().GetAsyncEnumerator();

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("1", enumerator.Current.FederalProvince.Key);
            Assert.Equal("Burgenland", enumerator.Current.FederalProvince.Name);
            Assert.Equal("101", enumerator.Current.Key);
            Assert.Equal("Eisenstadt(Stadt)", enumerator.Current.Name);
            Assert.Equal("101", enumerator.Current.Code);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("1", enumerator.Current.FederalProvince.Key);
            Assert.Equal("Burgenland", enumerator.Current.FederalProvince.Name);
            Assert.Equal("102", enumerator.Current.Key);
            Assert.Equal("Rust(Stadt)", enumerator.Current.Name);
            Assert.Equal("102", enumerator.Current.Code);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("9", enumerator.Current.FederalProvince.Key);
            Assert.Equal("Wien", enumerator.Current.FederalProvince.Name);
            Assert.Equal("900", enumerator.Current.Key);
            Assert.Equal("Wien 22.,Donaustadt", enumerator.Current.Name);
            Assert.Equal("922", enumerator.Current.Code);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("9", enumerator.Current.FederalProvince.Key);
            Assert.Equal("Wien", enumerator.Current.FederalProvince.Name);
            Assert.Equal("900", enumerator.Current.Key);
            Assert.Equal("Wien 23.,Liesing", enumerator.Current.Name);
            Assert.Equal("923", enumerator.Current.Code);

            Assert.False(await enumerator.MoveNextAsync());
        }

        [Fact]
        public async Task TestMunicipalityParsing()
        {
            using var csvStream = File.OpenText(Path.Combine(GetAssetsFolder(), "municipalities.csv"));

            var rdReader = new MunicipalityDataReader(csvStream);

            IAsyncEnumerator<Municipality> enumerator = rdReader.ReadAsync().GetAsyncEnumerator();

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("10101", enumerator.Current.Key);
            Assert.Equal("Eisenstadt", enumerator.Current.Name);
            Assert.Equal("10101", enumerator.Current.Code);
            Assert.Equal(MunicipalityStatus.SR, enumerator.Current.Status);
            Assert.Equal("7000", enumerator.Current.PostalCode);
            Assert.Empty(enumerator.Current.AdditionalPostalCodes);
            Assert.Equal("101", enumerator.Current.District.Code);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("10201", enumerator.Current.Key);
            Assert.Equal("Rust", enumerator.Current.Name);
            Assert.Equal("10201", enumerator.Current.Code);
            Assert.Equal(MunicipalityStatus.SR, enumerator.Current.Status);
            Assert.Equal("7071", enumerator.Current.PostalCode);
            Assert.Empty(enumerator.Current.AdditionalPostalCodes);
            Assert.Equal("102", enumerator.Current.District.Code);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("20201", enumerator.Current.Key);
            Assert.Equal("Villach", enumerator.Current.Name);
            Assert.Equal("20201", enumerator.Current.Code);
            Assert.Equal(MunicipalityStatus.SR, enumerator.Current.Status);
            Assert.Equal("9500", enumerator.Current.PostalCode);
            Assert.Equal(8, enumerator.Current.AdditionalPostalCodes.Length);
            Assert.Equal("202", enumerator.Current.District.Code);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("90001", enumerator.Current.Key);
            Assert.Equal("Wien", enumerator.Current.Name);
            Assert.Equal("92301", enumerator.Current.Code);
            Assert.Equal(MunicipalityStatus.SR, enumerator.Current.Status);
            Assert.Equal("1230", enumerator.Current.PostalCode);
            Assert.Empty(enumerator.Current.AdditionalPostalCodes);
            Assert.Equal("923", enumerator.Current.District.Code);

            Assert.False(await enumerator.MoveNextAsync());
        }

        [Fact]
        public async Task TestStreetParsing()
        {
            using var csvStream = File.OpenText(Path.Combine(GetAssetsFolder(), "streets.csv"));
            
            var rdReader = new StreetDataReader(csvStream);

            IAsyncEnumerator<Street> enumerator = rdReader.ReadAsync().GetAsyncEnumerator();

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("10101", enumerator.Current.Locality.Municipality.Key);
            Assert.Equal("Eisenstadt", enumerator.Current.Locality.Municipality.Name);
            Assert.Equal("10101", enumerator.Current.Locality.Municipality.Code);
            Assert.Equal("00001", enumerator.Current.Locality.Key);
            Assert.Equal("Eisenstadt", enumerator.Current.Locality.Name);
            Assert.Equal("7000", enumerator.Current.Locality.PostalCode);
            Assert.Equal("000001", enumerator.Current.Key);
            Assert.Equal("Josef Stanislaus Albach-Gasse", enumerator.Current.Name);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("10101", enumerator.Current.Locality.Municipality.Key);
            Assert.Equal("Eisenstadt", enumerator.Current.Locality.Municipality.Name);
            Assert.Equal("10101", enumerator.Current.Locality.Municipality.Code);
            Assert.Equal("00001", enumerator.Current.Locality.Key);
            Assert.Equal("Eisenstadt", enumerator.Current.Locality.Name);
            Assert.Equal("7000", enumerator.Current.Locality.PostalCode);
            Assert.Equal("000003", enumerator.Current.Key);
            Assert.Equal("Am Bahndamm", enumerator.Current.Name);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("10931", enumerator.Current.Locality.Municipality.Key);
            Assert.Equal("Badersdorf", enumerator.Current.Locality.Municipality.Name);
            Assert.Equal("10931", enumerator.Current.Locality.Municipality.Code);
            Assert.Equal("00262", enumerator.Current.Locality.Key);
            Assert.Equal("Badersdorf", enumerator.Current.Locality.Name);
            Assert.Equal("7512", enumerator.Current.Locality.PostalCode);
            Assert.Equal("134886", enumerator.Current.Key);
            Assert.Equal("Agrarweg", enumerator.Current.Name);

            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(new DateOnly(2022, 3, 3), enumerator.Current.TimeStamp);
            Assert.Equal("10932", enumerator.Current.Locality.Municipality.Key);
            Assert.Equal("Schandorf", enumerator.Current.Locality.Municipality.Name);
            Assert.Equal("10932", enumerator.Current.Locality.Municipality.Code);
            Assert.Equal("00301", enumerator.Current.Locality.Key);
            Assert.Equal("Schandorf / Cemba", enumerator.Current.Locality.Name);
            Assert.Equal("7472", enumerator.Current.Locality.PostalCode);
            Assert.Equal("076028", enumerator.Current.Key);
            Assert.Equal("Schandorf", enumerator.Current.Name);

            Assert.False(await enumerator.MoveNextAsync());
        }

        private static string GetAssetsFolder()
        {
            // Get the full location of the assembly
            string assemblyPath = System.Reflection.Assembly.GetAssembly(typeof(TestDataReaders)).Location;

            // Get the folder that's in
            return Path.Combine(Path.GetDirectoryName(assemblyPath), "at", "Assets");
        }
    }
}
