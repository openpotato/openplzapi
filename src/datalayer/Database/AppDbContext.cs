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

using Microsoft.EntityFrameworkCore;

namespace OpenPlzApi.DataLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AT.FederalProvince>();
            modelBuilder.Entity<AT.District>();
            modelBuilder.Entity<AT.Municipality>();
            modelBuilder.Entity<AT.Locality>();
            modelBuilder.Entity<AT.Street>();
            modelBuilder.Entity<AT.FullTextStreet>()
                .HasGeneratedTsVectorColumn(
                    x => x.SearchVector, "config_openplzapi", x => new { x.Name, x.PostalCode, x.Locality })
                .HasIndex(p => p.SearchVector)
                .HasMethod("GIN");

            modelBuilder.Entity<CH.Canton>();
            modelBuilder.Entity<CH.District>();
            modelBuilder.Entity<CH.Commune>();
            modelBuilder.Entity<CH.Locality>();
            modelBuilder.Entity<CH.Street>();
            modelBuilder.Entity<CH.FullTextStreet>()
                .HasGeneratedTsVectorColumn(
                    x => x.SearchVector, "config_openplzapi", x => new { x.Name, x.PostalCode, x.Locality })
                .HasIndex(p => p.SearchVector)
                .HasMethod("GIN");

            modelBuilder.Entity<DE.FederalState>();
            modelBuilder.Entity<DE.GovernmentRegion>();
            modelBuilder.Entity<DE.District>();
            modelBuilder.Entity<DE.MunicipalAssociation>();
            modelBuilder.Entity<DE.Municipality>();
            modelBuilder.Entity<DE.Locality>();
            modelBuilder.Entity<DE.Street>();
            modelBuilder.Entity<DE.FullTextStreet>()
                .HasGeneratedTsVectorColumn(
                    x => x.SearchVector, "config_openplzapi", x => new { x.Name, x.PostalCode, x.Locality })
                .HasIndex(p => p.SearchVector)
                .HasMethod("GIN");

            modelBuilder.Entity<LI.Commune>();
            modelBuilder.Entity<LI.Locality>();
            modelBuilder.Entity<LI.Street>();
            modelBuilder.Entity<LI.FullTextStreet>()
                .HasGeneratedTsVectorColumn(
                    x => x.SearchVector, "config_openplzapi", x => new { x.Name, x.PostalCode, x.Locality })  
                .HasIndex(p => p.SearchVector)
                .HasMethod("GIN"); 
        }
    }
}
