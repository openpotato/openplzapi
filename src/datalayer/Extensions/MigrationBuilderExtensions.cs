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

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace OpenPlzAPI.DataLayer.Migrations
{
    public static class MigrationBuilderExtensions
    {
        public static OperationBuilder<SqlOperation> CreateTextSearchConfiguration(this MigrationBuilder migrationBuilder, string name)
        {
            return migrationBuilder.Sql(
                $"CREATE TEXT SEARCH CONFIGURATION {name} ( copy = german );" +
                $"ALTER TEXT SEARCH CONFIGURATION {name} ALTER MAPPING FOR hword, hword_part, word WITH unaccent, german_stem;");
        }

        public static OperationBuilder<SqlOperation> CreateUnaccentExtension(this MigrationBuilder migrationBuilder)
        {
            return migrationBuilder.Sql("CREATE EXTENSION unaccent");
        }

        public static OperationBuilder<SqlOperation> DropTextSearchConfiguration(this MigrationBuilder migrationBuilder, string name)
        {
            return migrationBuilder.Sql($"DROP TEXT SEARCH CONFIGURATION {name}");
        }

        public static OperationBuilder<SqlOperation> DropUnaccentExtension(this MigrationBuilder migrationBuilder)
        {
            return migrationBuilder.Sql("DROP EXTENSION unaccent");
        }
    }
}