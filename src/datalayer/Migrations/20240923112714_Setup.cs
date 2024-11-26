using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenPlzAPI.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Setup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateUnaccentExtension();
            migrationBuilder.CreateTextSearchConfiguration("config_openplzapi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTextSearchConfiguration("config_openplzapi");
            migrationBuilder.DropUnaccentExtension();
        }
    }
}
