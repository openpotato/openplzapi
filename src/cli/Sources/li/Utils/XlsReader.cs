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
using System.Globalization;

namespace OpenPlzApi.CLI.Sources.LI
{
    public class XlsReader
    {
        private readonly int _firstRowNumber;
        private readonly int _lastRowNumber;
        private readonly IXLWorksheet _worksheet;
        private IXLRow _currentRow;

        public XlsReader(IXLWorkbook xlsDocument, string xlsSheetName, int? xlsFirstRowNumber, int? xlsLastRowNumber)
        {
            if (xlsDocument.Worksheets.Count > 0)
            {
                // Try to get worksheet
                if (!string.IsNullOrEmpty(xlsSheetName) && xlsDocument.TryGetWorksheet(xlsSheetName, out var _foundWorksheet))
                {
                    _worksheet = _foundWorksheet;
                }
                else
                {
                    _worksheet = xlsDocument.Worksheet(1);
                }

                // Try to get first an last row number of data list
                _firstRowNumber = xlsFirstRowNumber ?? _worksheet.FirstRowUsed().RowNumber();
                _lastRowNumber = xlsLastRowNumber ?? _worksheet.LastRowUsed().RowNumber();
            }
            else
            {
                throw new XlsReaderException("The Excel file seems to have no worksheet");
            }
        }

        public int CurrentRowNumber { get; private set; } = 0;

        public DateOnly? GetDateOnlyValue(string columnName)
        {
            if (!string.IsNullOrEmpty(columnName))
            {
                var strValue = _currentRow.GetCellValue<string>(columnName);
                {
                    if (DateOnly.TryParseExact(strValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    {
                        return date;
                    }
                    else
                    {
                        return default;
                    }
                }
            }
            else
            {
                return default;
            }
        }

        public string GetStringValue(string columnName)
        {
            if (!string.IsNullOrEmpty(columnName))
            {
                return _currentRow.GetCellValue<string>(columnName);
            }
            else
            {
                return default;
            }
        }

        public bool ReadLine()
        {
            if (CurrentRowNumber == 0)
            {
                CurrentRowNumber = _firstRowNumber - 1;
            }

            while (CurrentRowNumber < _lastRowNumber)
            {
                CurrentRowNumber++;
                _currentRow = _worksheet.Row(CurrentRowNumber);

                if (!_currentRow.IsEmpty())
                {
                    return true;
                }
            }

            return false;
        }
    }
}