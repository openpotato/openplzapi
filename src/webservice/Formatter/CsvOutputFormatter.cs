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

using Enbrea.Csv;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace OpenPlzApi
{
    /// <summary>
    /// Writes <see cref="BaseResponse"/> instances formatted as CSV to the output stream.
    /// </summary>
    public class CsvOutputFormatter : TextOutputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvOutputFormatter"/> class.
        /// </summary>
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        /// <summary>
        /// Writes the response body.
        /// </summary>
        /// <param name="context">The formatter context associated with the call.</param>
        /// <param name="selectedEncoding">The <see cref="Encoding"/> that should be used to write the response.</param>
        /// <returns>A task which can write the response body.</returns>
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var buffer = new StringBuilder();

            using var strWriter = new StringWriter(buffer);

            var csvWriter = new CsvTableWriter(strWriter);
            csvWriter.SetTrueFalseString<bool>("true", "false");

            if (context.Object is IEnumerable<AT.FederalProvinceResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<AT.FederalProvinceResponse>);
            }
            else if (context.Object is IEnumerable<AT.DistrictResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<AT.DistrictResponse>);
            }
            else if (context.Object is IEnumerable<AT.MunicipalityResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<AT.MunicipalityResponse>);
            }
            else if (context.Object is IEnumerable<AT.LocalityResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<AT.LocalityResponse>);
            }
            else if (context.Object is IEnumerable<AT.StreetResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<AT.StreetResponse>);
            }
            else if (context.Object is IEnumerable<CH.CantonResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<CH.CantonResponse>);
            }
            else if (context.Object is IEnumerable<CH.DistrictResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<CH.DistrictResponse>);
            }
            else if (context.Object is IEnumerable<CH.CommuneResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<CH.CommuneResponse>);
            }
            else if (context.Object is IEnumerable<CH.LocalityResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<CH.LocalityResponse>);
            }
            else if (context.Object is IEnumerable<CH.StreetResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<CH.StreetResponse>);
            }
            else if (context.Object is IEnumerable <DE.FederalStateResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<DE.FederalStateResponse>);
            }
            else if (context.Object is IEnumerable<DE.LocalityResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<DE.LocalityResponse>);
            }
            else if (context.Object is IEnumerable<DE.StreetResponse>)
            {
                WriteResponse(csvWriter, context.Object as IEnumerable<DE.StreetResponse>);
            }

            await context.HttpContext.Response.WriteAsync(buffer.ToString(), selectedEncoding);
        }

        /// <summary>
        /// Returns a value indicating whether or not the given type can be written by this serializer.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>TRUE if the type can be written, otherwise FALSE.</returns>
        protected override bool CanWriteType(Type type)
        {
            return
                typeof(IEnumerable<AT.FederalProvinceResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<AT.DistrictResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<AT.MunicipalityResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<AT.LocalityResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<AT.StreetResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<CH.CantonResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<CH.DistrictResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<CH.CommuneResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<CH.LocalityResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<CH.StreetResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<DE.FederalStateResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<DE.GovernmentRegionResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<DE.DistrictResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<DE.MunicipalAssociationResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<DE.MunicipalityResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<DE.LocalityResponse>).IsAssignableFrom(type) ||
                typeof(IEnumerable<DE.StreetResponse>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Writes a list of Austrian federal provinces to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="federalProvinces">The list of federal provinces</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<AT.FederalProvinceResponse> federalProvinces)
        {
            csvWriter.WriteHeaders(
                "Key",
                "Name");

            foreach (var federalProvince in federalProvinces)
            {
                csvWriter.SetValue("Key", federalProvince.Key);
                csvWriter.SetValue("Name", federalProvince.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of Austrian districts to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="districts">The list of districts</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<AT.DistrictResponse> districts)
        {
            csvWriter.WriteHeaders(
                "Key",
                "PostalCode",
                "Name",
                "Municipality.Key",
                "Municipality.Code",
                "Municipality.Name",
                "District.Key",
                "District.Code",
                "District.Name",
                "FederalProvince.Key",
                "FederalProvince.Name");

            foreach (var district in districts)
            {
                csvWriter.SetValue("Key", district.Key);
                csvWriter.SetValue("PostalCode", district.Code);
                csvWriter.SetValue("Name", district.Name);
                csvWriter.SetValue("FederalProvince.Key", district.FederalProvince.Key);
                csvWriter.SetValue("FederalProvince.Name", district.FederalProvince.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of Austrian municipalities to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="municipalities">The list of municipalities</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<AT.MunicipalityResponse> municipalities)
        {
            csvWriter.WriteHeaders(
                "Key",
                "Code",
                "Name",
                "PostalCode",
                "MultiplePostalCodes",
                "Status",
                "District.Key",
                "District.Code",
                "District.Name",
                "FederalProvince.Key",
                "FederalProvince.Name");

            foreach (var municipality in municipalities)
            {
                csvWriter.SetValue("Key", municipality.Key);
                csvWriter.SetValue("Code", municipality.Code);
                csvWriter.SetValue("Name", municipality.Name);
                csvWriter.SetValue("PostalCode", municipality.PostalCode);
                csvWriter.SetValue("MultiplePostalCodes", municipality.MultiplePostalCodes);
                csvWriter.SetValue("Status", municipality.Status);
                csvWriter.SetValue("District.Key", municipality.District.Key);
                csvWriter.SetValue("District.Code", municipality.District.Code);
                csvWriter.SetValue("District.Name", municipality.District.Name);
                csvWriter.SetValue("FederalProvince.Key", municipality.FederalProvince.Key);
                csvWriter.SetValue("FederalProvince.Name", municipality.FederalProvince.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of Austrian localities to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="localities">The list of localities</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<AT.LocalityResponse> localities)
        {
            csvWriter.WriteHeaders(
                "Key",
                "PostalCode",
                "Name",
                "Municipality.Key",
                "Municipality.Code",
                "Municipality.Name",
                "District.Key",
                "District.Code",
                "District.Name",
                "FederalProvince.Key",
                "FederalProvince.Name");

            foreach (var localitiy in localities)
            {
                csvWriter.SetValue("Key", localitiy.Key);
                csvWriter.SetValue("PostalCode", localitiy.PostalCode);
                csvWriter.SetValue("Name", localitiy.Name);
                csvWriter.SetValue("Municipality.Key", localitiy.Municipality.Key);
                csvWriter.SetValue("Municipality.Code", localitiy.Municipality.Code);
                csvWriter.SetValue("Municipality.Name", localitiy.Municipality.Name);
                csvWriter.SetValue("District.Key", localitiy.District.Key);
                csvWriter.SetValue("District.Code", localitiy.District.Code);
                csvWriter.SetValue("District.Name", localitiy.District.Name);
                csvWriter.SetValue("FederalProvince.Key", localitiy.FederalProvince.Key);
                csvWriter.SetValue("FederalProvince.Name", localitiy.FederalProvince.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of Austrian streets to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="streets">The list of streets</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<AT.StreetResponse> streets)
        {
            csvWriter.WriteHeaders(
                "Key",
                "Name",
                "PostalCode",
                "Locality",
                "Municipality.Key",
                "Municipality.Code",
                "Municipality.Name",
                "District.Key",
                "District.Code",
                "District.Name",
                "FederalProvince.Key",
                "FederalProvince.Name");

            foreach (var street in streets)
            {
                csvWriter.SetValue("Key", street.Key);
                csvWriter.SetValue("Name", street.Name);
                csvWriter.SetValue("PostalCode", street.PostalCode);
                csvWriter.SetValue("Locality", street.Locality);
                csvWriter.SetValue("Municipality.Key", street.Municipality.Key);
                csvWriter.SetValue("Municipality.Code", street.Municipality.Code);
                csvWriter.SetValue("Municipality.Name", street.Municipality.Name);
                csvWriter.SetValue("District.Key", street.District.Key);
                csvWriter.SetValue("District.Code", street.District.Code);
                csvWriter.SetValue("District.Name", street.District.Name);
                csvWriter.SetValue("FederalProvince.Key", street.FederalProvince.Key);
                csvWriter.SetValue("FederalProvince.Name", street.FederalProvince.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of Swiss cantons to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="cantons">The list of cantons</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<CH.CantonResponse> cantons)
        {
            csvWriter.WriteHeaders(
                "Key",
                "Name");

            foreach (var canton in cantons)
            {
                csvWriter.SetValue("Key", canton.Key);
                csvWriter.SetValue("Name", canton.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of Swiss districts to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="districts">The list of districts</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<CH.DistrictResponse> districts)
        {
            csvWriter.WriteHeaders(
                "Key",
                "Name",
                "Canton.Key",
                "Canton.Name");

            foreach (var district in districts)
            {
                csvWriter.SetValue("Key", district.Key);
                csvWriter.SetValue("Name", district.Name);
                csvWriter.SetValue("Canton.Key", district.Canton.Key);
                csvWriter.SetValue("Canton.Name", district.Canton.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of Swiss communes to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="communes">The list of communes</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<CH.CommuneResponse> communes)
        {
            csvWriter.WriteHeaders(
                "Key",
                "Name",
                "ShortName",
                "District.Key",
                "District.Name",
                "Canton.Key",
                "Canton.Name");

            foreach (var commune in communes)
            {
                csvWriter.SetValue("Key", commune.Key);
                csvWriter.SetValue("Name", commune.Name);
                csvWriter.SetValue("ShortName", commune.ShortName);
                csvWriter.SetValue("District.Key", commune.District.Key);
                csvWriter.SetValue("District.Name", commune.District.Name);
                csvWriter.SetValue("Canton.Key", commune.Canton.Key);
                csvWriter.SetValue("Canton.Name", commune.Canton.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of Swiss localities to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="localities">The list of localities</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<CH.LocalityResponse> localities)
        {
            csvWriter.WriteHeaders(
                "PostalCode",
                "Name",
                "Commune.Code",
                "Commune.Name",
                "Commune.ShortName",
                "District.Key",
                "District.Name",
                "Canton.Key",
                "Canton.Name");

            foreach (var localitiy in localities)
            {
                csvWriter.SetValue("PostalCode", localitiy.PostalCode);
                csvWriter.SetValue("Name", localitiy.Name);
                csvWriter.SetValue("Commune.Key", localitiy.Commune.Key);
                csvWriter.SetValue("Commune.Name", localitiy.Commune.Name);
                csvWriter.SetValue("Commune.ShortName", localitiy.Commune.ShortName);
                csvWriter.SetValue("District.Key", localitiy.District.Key);
                csvWriter.SetValue("District.Name", localitiy.District.Name);
                csvWriter.SetValue("Canton.Key", localitiy.Canton.Key);
                csvWriter.SetValue("Canton.Name", localitiy.Canton.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of Swiss streets to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="streets">The list of streets</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<CH.StreetResponse> streets)
        {
            csvWriter.WriteHeaders(
                "Key",
                "Name",
                "PostalCode",
                "Locality",
                "Status",
                "Commune.Key",
                "Commune.Name",
                "District.Key",
                "District.Name",
                "Canton.Key",
                "Canton.Name");

            foreach (var street in streets)
            {
                csvWriter.SetValue("Key", street.Key);
                csvWriter.SetValue("Name", street.Name);
                csvWriter.SetValue("PostalCode", street.PostalCode);
                csvWriter.SetValue("Locality", street.Locality);
                csvWriter.SetValue("Status", street.Status);
                csvWriter.SetValue("Commune.Key", street.Commune.Key);
                csvWriter.SetValue("Commune.Name", street.Commune.Name);
                csvWriter.SetValue("District.Key", street.District.Key);
                csvWriter.SetValue("District.Name", street.District.Name);
                csvWriter.SetValue("Canton.Key", street.Canton.Key);
                csvWriter.SetValue("Canton.Name", street.Canton.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of German federal states to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="federalStates">The list of federal states</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<DE.FederalStateResponse> federalStates)
        {
            csvWriter.WriteHeaders(
                "Key",
                "Name",
                "SeatOfGovernment");

            foreach (var federalState in federalStates)
            {
                csvWriter.SetValue("Key", federalState.Key);
                csvWriter.SetValue("Name", federalState.Name);
                csvWriter.SetValue("SeatOfGovernment", federalState.SeatOfGovernment);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of German localities to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="localities">The list of localities</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<DE.LocalityResponse> localities)
        {
            csvWriter.WriteHeaders(
                "PostalCode",
                "Name",
                "Municipality.Key",
                "Municipality.Name",
                "Municipality.Type",
                "District.Key",
                "District.Name",
                "District.Type",
                "FederalState.Key",
                "FederalState.Name");

            foreach (var localitiy in localities)
            {
                csvWriter.SetValue("PostalCode", localitiy.PostalCode);
                csvWriter.SetValue("Name", localitiy.Name);
                csvWriter.SetValue("Municipality.Key", localitiy.Municipality.Key);
                csvWriter.SetValue("Municipality.Name", localitiy.Municipality.Name);
                csvWriter.SetValue("Municipality.Type", localitiy.Municipality.Type);
                csvWriter.SetValue("District.Key", localitiy.District.Key);
                csvWriter.SetValue("District.Name", localitiy.District.Name);
                csvWriter.SetValue("District.Type", localitiy.District.Type);
                csvWriter.SetValue("FederalState.Key", localitiy.FederalState.Key);
                csvWriter.SetValue("FederalState.Name", localitiy.FederalState.Name);

                csvWriter.Write();
            }
        }

        /// <summary>
        /// Writes a list of German streets to the CSV writer.
        /// </summary>
        /// <param name="csvWriter">The CSV writer</param>
        /// <param name="streets">The list of streets</param>
        private void WriteResponse(CsvTableWriter csvWriter, IEnumerable<DE.StreetResponse> streets)
        {
            csvWriter.WriteHeaders(
                "Name",
                "PostalCode",
                "Locality",
                "Municipality.Key",
                "Municipality.Name",
                "Municipality.Type",
                "District.Key",
                "District.Name",
                "District.Type",
                "FederalState.Key",
                "FederalState.Name");

            foreach (var street in streets)
            {
                csvWriter.SetValue("Name", street.Name);
                csvWriter.SetValue("PostalCode", street.PostalCode);
                csvWriter.SetValue("Locality", street.Locality);
                csvWriter.SetValue("Municipality.Key", street.Municipality.Key);
                csvWriter.SetValue("Municipality.Name", street.Municipality.Name);
                csvWriter.SetValue("Municipality.Type", street.Municipality.Type);
                csvWriter.SetValue("District.Key", street.District.Key);
                csvWriter.SetValue("District.Name", street.District.Name);
                csvWriter.SetValue("District.Type", street.District.Type);
                csvWriter.SetValue("FederalState.Key", street.FederalState.Key);
                csvWriter.SetValue("FederalState.Name", street.FederalState.Name);

                csvWriter.Write();
            }
        }
    }
}
 