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

using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OpenPlzApi.CLI.Sources.CH
{
    /// <summary>
    /// A loader for the Swiss official commune register
    /// </summary>
    public class CommuneRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommuneRegister"/>.
        /// </summary>
        public CommuneRegister()
        {
            Cantons = [];
            Communes = [];
            Districts = [];
        }

        /// <summary>
        /// List of cantons 
        /// </summary>
        public IList<Canton> Cantons { get; }

        /// <summary>
        /// List of communes
        /// </summary>
        public IList<Commune> Communes { get; }

        /// <summary>
        /// List of distrcits 
        /// </summary>
        public IList<District> Districts { get; }

        /// <summary>
        /// Time stamp of the register
        /// </summary>
        public DateOnly? TimeStamp { get; internal set; }

        /// <summary>
        /// Remove all loaded data
        /// </summary>
        public void Clear()
        {
            TimeStamp = null;
            Communes.Clear();
            Districts.Clear();
            Cantons.Clear();
        }

        /// <summary>
        /// Loads the Swiss official commune register from the official Excel file.
        /// </summary>
        /// <param name="stream">File stream</param>
        public void Load(Stream stream)
        {
            Clear();

            using var xlsDocument = new XLWorkbook(stream);

            var firstWorksheetName = xlsDocument.Worksheets.Worksheet(1).Name;

            TimeStamp = DateOnly.ParseExact(firstWorksheetName, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            LoadCantons(xlsDocument);
            LoadDistricts(xlsDocument);
            LoadCommunes(xlsDocument);
        }

        /// <summary>
        /// Loads all cantons from the given Excel document
        /// </summary>
        /// <param name="xlsDocument">Excel document</param>
        private void LoadCantons(XLWorkbook xlsDocument)
        {
            var xlsReader = new XlsReader(xlsDocument, "KT", 2, null);

            while (xlsReader.ReadLine())
            {
                Cantons.Add(new Canton()
                {
                    Key = xlsReader.GetStringValue("A"),
                    Code = xlsReader.GetStringValue("B"),
                    Name = xlsReader.GetStringValue("C")
                });
            }
        }

        /// <summary>
        /// Loads all communes from the given Excel document
        /// </summary>
        /// <param name="xlsDocument">Excel document</param>
        private void LoadCommunes(XLWorkbook xlsDocument)
        {
            var xlsReader = new XlsReader(xlsDocument, "GDE", 2, null);

            while (xlsReader.ReadLine())
            {
                Communes.Add(new Commune()
                {
                    Canton = Cantons.FirstOrDefault(c => c.Code == xlsReader.GetStringValue("A")),
                    District = Districts.FirstOrDefault(c => c.Key == xlsReader.GetStringValue("B")),
                    Key = xlsReader.GetStringValue("C"),
                    Name = xlsReader.GetStringValue("D"),
                    ShortName = xlsReader.GetStringValue("E"),
                    LastModified = xlsReader.GetDateOnlyValue("H")
                });
            }
        }

        /// <summary>
        /// Loads all districts from the given Excel document
        /// </summary>
        /// <param name="xlsDocument">Excel document</param>
        private void LoadDistricts(XLWorkbook xlsDocument)
        {
            var xlsReader = new XlsReader(xlsDocument, "BZN", 2, null);

            while (xlsReader.ReadLine())
            {
                Districts.Add(new District()
                {
                    Key = xlsReader.GetStringValue("B"),
                    Name = xlsReader.GetStringValue("C"),
                    Canton = Cantons.FirstOrDefault(c => c.Code == xlsReader.GetStringValue("A"))
                });
            }
        }
    }
}
