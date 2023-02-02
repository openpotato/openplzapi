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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OpenPlzApi.DataLayer;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenPlzApi.CLI
{
    /// <summary>
    /// Manager for importing raw data to database
    /// </summary>
    public class ImportManager
    {
        private readonly AppConfiguration _appConfiguration = new();
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportManager"/> class.
        /// </summary>
        /// <param name="appConfiguration">Configuration data</param>
        public ImportManager(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            _dbContextFactory = new PooledDbContextFactory<AppDbContext>(AppDbContextOptionsFactory.CreateDbContextOptions(_appConfiguration.Database));
        }

        /// <summary>
        /// Executes the data import
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A task that represents the asynchronous import operation.</returns>
        public async Task ExecuteAsync(ImportSource source, CancellationToken cancellationToken)
        {
            switch (source)
            {
                case ImportSource.AT:
                    await ImportATDistricts(cancellationToken);
                    await ImportATMunicipalities(cancellationToken);
                    await ImportATStreets(cancellationToken);
                    break;

                case ImportSource.CH:
                    await ImportCHCommunes(cancellationToken);
                    await ImportCHStreets(cancellationToken);
                    break;

                case ImportSource.DE:
                    await ImportDEMunicipalities(cancellationToken);
                    await ImportDEStreets(cancellationToken);
                    break;

                default:
                    break;
            };

        }

        private async Task ImportATDistricts(CancellationToken cancellationToken)
        {
            var importer = new AT.DistrictsImporter(_dbContextFactory,
                _appConfiguration.Sources.AT.Districts.Caption,
                _appConfiguration.Sources.AT.Districts.RemoteSourceFile,
                new FileInfo(Path.Combine(_appConfiguration.Sources.RootFolderName, _appConfiguration.Sources.AT.Districts.LocalSourceFileName)));

            await importer.ExecuteAsync(cancellationToken);
        }

        private async Task ImportATMunicipalities(CancellationToken cancellationToken)
        {
            var importer = new AT.MunicipalitiesImporter(_dbContextFactory, 
                _appConfiguration.Sources.AT.Municipalities.Caption,
                _appConfiguration.Sources.AT.Municipalities.RemoteSourceFile,
                new FileInfo(Path.Combine(_appConfiguration.Sources.RootFolderName, _appConfiguration.Sources.AT.Municipalities.LocalSourceFileName)));

            await importer.ExecuteAsync(cancellationToken);
        }

        private async Task ImportATStreets(CancellationToken cancellationToken)
        {
            foreach (var streetConfig in _appConfiguration.Sources.AT.Streets)
            {
                var importer = new AT.StreetsImporter(_dbContextFactory, 
                    streetConfig.Caption,
                    streetConfig.RemoteSourceFile,
                    new FileInfo(Path.Combine(_appConfiguration.Sources.RootFolderName, streetConfig.LocalSourceFileName)));

                await importer.ExecuteAsync(cancellationToken);
            }
        }

        private async Task ImportCHCommunes(CancellationToken cancellationToken)
        {
            var importer = new CH.CommunesImporter(_dbContextFactory,
                _appConfiguration.Sources.CH.Communes.Caption,
                _appConfiguration.Sources.CH.Communes.RemoteSourceFile,
                new FileInfo(Path.Combine(_appConfiguration.Sources.RootFolderName, _appConfiguration.Sources.CH.Communes.LocalSourceFileName)));

            await importer.ExecuteAsync(cancellationToken);
        }

        private async Task ImportCHStreets(CancellationToken cancellationToken)
        {
            var importer = new CH.StreetsImporter(_dbContextFactory,
                _appConfiguration.Sources.CH.Streets.Caption,
                _appConfiguration.Sources.CH.Streets.RemoteZipArchive,
                new FileInfo(Path.Combine(_appConfiguration.Sources.RootFolderName, _appConfiguration.Sources.CH.Streets.LocalZipArchiveName)),
                new FileInfo(Path.Combine(_appConfiguration.Sources.RootFolderName, _appConfiguration.Sources.CH.Streets.LocalSourceFileName)));

            await importer.ExecuteAsync(cancellationToken);
        }

        private async Task ImportDEMunicipalities(CancellationToken cancellationToken)
        {
            var importer = new DE.MunicipalitiesImporter(_dbContextFactory,
                _appConfiguration.Sources.DE.Municipalities.Caption,
                _appConfiguration.Sources.DE.Municipalities.RemoteZipArchive,
                new FileInfo(Path.Combine(_appConfiguration.Sources.RootFolderName, _appConfiguration.Sources.DE.Municipalities.LocalZipArchiveName)),
                new FileInfo(Path.Combine(_appConfiguration.Sources.RootFolderName, _appConfiguration.Sources.DE.Municipalities.LocalSourceFileName)));

            await importer.ExecuteAsync(cancellationToken);
        }

        private async Task ImportDEStreets(CancellationToken cancellationToken)
        {
            var importer = new DE.StreetsImporter(_dbContextFactory,
                _appConfiguration.Sources.DE.Streets.Caption,
                _appConfiguration.Sources.DE.Streets.RemoteSourceFile,
                new FileInfo(Path.Combine(_appConfiguration.Sources.RootFolderName, _appConfiguration.Sources.DE.Streets.LocalSourceFileName)));

            await importer.ExecuteAsync(cancellationToken);
        }
    }
}
