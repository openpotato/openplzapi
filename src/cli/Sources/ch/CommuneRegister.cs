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

using OpenPlzApi.AGVCH;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        /// Remove all loaded data
        /// </summary>
        public void Clear()
        {
            Communes.Clear();
            Districts.Clear();
            Cantons.Clear();
        }

        /// <summary>
        /// Loads the Swiss official commune register from the official Excel file.
        /// </summary>
        /// <param name="stream">File stream</param>
        public async Task Load(Stream stream)
        {
            Clear();

            var snapshot = AGVCHReader.ReadAsync<SnapshotRecord>(stream);

            await foreach (var snapshotRecord in snapshot)
            {
                if (snapshotRecord.Level == SnapshotLevel.Canton)
                {
                    Cantons.Add(new Canton()
                    {
                        Key = snapshotRecord.BfsCode,
                        HistoricalCode = snapshotRecord.HistoricalCode,
                        ShortName = snapshotRecord.ShortName,
                        Name = snapshotRecord.Name,
                    });
                }
                else if (snapshotRecord.Level == SnapshotLevel.District)
                {
                    Districts.Add(new District()
                    {
                        Key = snapshotRecord.BfsCode,
                        HistoricalCode = snapshotRecord.HistoricalCode,
                        ShortName = snapshotRecord.ShortName,
                        Name = snapshotRecord.Name,
                        Canton = Cantons.FirstOrDefault(c => c.HistoricalCode == snapshotRecord.Parent)
                    });
                }
                else if (snapshotRecord.Level == SnapshotLevel.Commune)
                {
                    Communes.Add(new Commune()
                    {
                        Key = snapshotRecord.BfsCode,
                        HistoricalCode = snapshotRecord.HistoricalCode,
                        Name = snapshotRecord.Name,
                        ShortName = snapshotRecord.ShortName,
                        District = Districts.FirstOrDefault(c => c.HistoricalCode == snapshotRecord.Parent),
                        Canton = Districts.FirstOrDefault(c => c.HistoricalCode == snapshotRecord.Parent)?.Canton
                    });
                }
            }
        }
    }
}
