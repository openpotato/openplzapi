using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace OpenPlzAPI.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ch");

            migrationBuilder.EnsureSchema(
                name: "li");

            migrationBuilder.EnsureSchema(
                name: "at");

            migrationBuilder.EnsureSchema(
                name: "de");

            migrationBuilder.CreateTable(
                name: "Cantons",
                schema: "ch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    HistoricalCode = table.Column<string>(type: "text", nullable: false, comment: "Historical code (Historisierte Nummer des Kantons)"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Bfs-Nummer des Kantons)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Kantonsname)"),
                    ShortName = table.Column<string>(type: "text", nullable: false, comment: "Short name (Kantonskürzel)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cantons", x => x.Id);
                },
                comment: "Representation of a Swiss canton (Kanton)");

            migrationBuilder.CreateTable(
                name: "Communes",
                schema: "li",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    ElectoralDistrict = table.Column<string>(type: "text", nullable: false, comment: "Electoral district (Wahlkreis)"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Gemeindenummer)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Amtlicher Gemeindename)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communes", x => x.Id);
                },
                comment: "Representation of a Liechtenstein commune (Gemeinde)");

            migrationBuilder.CreateTable(
                name: "FederalProvinces",
                schema: "at",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Bundeslandkennziffer)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Bundeslandname)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederalProvinces", x => x.Id);
                },
                comment: "Representation of an Austrian federal province (Bundesland)");

            migrationBuilder.CreateTable(
                name: "FederalStates",
                schema: "de",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Regional key (Regionalschlüssel)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Bundeslandname)"),
                    SeatOfGovernment = table.Column<string>(type: "text", nullable: true, comment: "Seat of government (Sitz der Landesregierung)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederalStates", x => x.Id);
                },
                comment: "Representation of a German federal state (Bundesland)");

            migrationBuilder.CreateTable(
                name: "Districts",
                schema: "ch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    HistoricalCode = table.Column<string>(type: "text", nullable: false, comment: "Historical code (Historisierte Nummer des Bezirks)"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Bfs-Nummer des Bezirks)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Bezirksname)"),
                    ShortName = table.Column<string>(type: "text", nullable: false, comment: "Short name (Bezirksname, kurz)"),
                    CantonId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to canton (Kanton)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Cantons_CantonId",
                        column: x => x.CantonId,
                        principalSchema: "ch",
                        principalTable: "Cantons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a Swiss district (Bezirk)");

            migrationBuilder.CreateTable(
                name: "FullTextStreets",
                schema: "li",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Straßenschlüssel)"),
                    Locality = table.Column<string>(type: "text", nullable: false, comment: "Locality"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Straßenname)"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, comment: "Postal code (Postleitzahl)"),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true, comment: "tsvector column for full text search")
                        .Annotation("Npgsql:TsVectorConfig", "config_openplzapi")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "PostalCode", "Locality" }),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Status (Straßenstatus)"),
                    CommuneId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to commune (Gemeinde)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullTextStreets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FullTextStreets_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalSchema: "li",
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a Liechtenstein street (Straße) for full text search");

            migrationBuilder.CreateTable(
                name: "Localities",
                schema: "li",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Ortsname)"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, comment: "Postal code (Postleitzahl)"),
                    CommuneId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to commune (Gemeinde)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localities_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalSchema: "li",
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a Liechtenstein locality (Ort oder Stadt)");

            migrationBuilder.CreateTable(
                name: "Districts",
                schema: "at",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Code = table.Column<string>(type: "text", nullable: false, comment: "Code (Bezirkskodierung)"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Bezirkskennziffer)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Bezirksname)"),
                    FederalProvinceId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to federal province (Bundesland)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_FederalProvinces_FederalProvinceId",
                        column: x => x.FederalProvinceId,
                        principalSchema: "at",
                        principalTable: "FederalProvinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of an Austrian district (Politischer Bezirk)");

            migrationBuilder.CreateTable(
                name: "GovernmentRegions",
                schema: "de",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    AdministrativeHeadquarters = table.Column<string>(type: "text", nullable: true, comment: "Administrative headquarters (Verwaltungssitz des Regierungsbezirks)"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Regional key (Regionalschlüssel)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Bezirksname)"),
                    FederalStateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to federal state (Bundesland)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentRegions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovernmentRegions_FederalStates_FederalStateId",
                        column: x => x.FederalStateId,
                        principalSchema: "de",
                        principalTable: "FederalStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a German government region (Regierungsbezirk)");

            migrationBuilder.CreateTable(
                name: "Communes",
                schema: "ch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    HistoricalCode = table.Column<string>(type: "text", nullable: false, comment: "Historical code (Historisierte Nummer der Gemeinde)"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Bfs-Gemeindenummer)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Amtlicher Gemeindename)"),
                    ShortName = table.Column<string>(type: "text", nullable: false, comment: "Short name (Gemeindename, kurz)"),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to district (Bezirk)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communes_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "ch",
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a Swiss commune (Gemeinde)");

            migrationBuilder.CreateTable(
                name: "Streets",
                schema: "li",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Straßenschlüssel)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Straßenname)"),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Status (Straßenstatus)"),
                    LocalityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to locality")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streets_Localities_LocalityId",
                        column: x => x.LocalityId,
                        principalSchema: "li",
                        principalTable: "Localities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a Liechtenstein street (Straße)");

            migrationBuilder.CreateTable(
                name: "Municipalities",
                schema: "at",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Code = table.Column<string>(type: "text", nullable: false, comment: "Code (Gemeindecode)"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Gemeindekennziffer)"),
                    MultiplePostalCodes = table.Column<bool>(type: "boolean", nullable: false, comment: "This municipality has multiple postal codes?"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Ortschaftsname)"),
                    PostalCode = table.Column<string>(type: "text", nullable: true, comment: "Postal code (Postleitzahl des Gemeindeamtes)"),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Status (Gemeindestatus)"),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to district (Bezirk)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipalities_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "at",
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of an Austrian municipality (Gemeinde)");

            migrationBuilder.CreateTable(
                name: "Districts",
                schema: "de",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    AdministrativeHeadquarters = table.Column<string>(type: "text", nullable: true, comment: "Administrative headquarters (Sitz der Kreisverwaltung)"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Regional key (Regionalschlüssel)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Kreisname)"),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Type (Kreiskennzeichen)"),
                    FederalStateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to federal state (Bundesland)"),
                    GovernmentRegionId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Reference to government region (Regierungsbezirk)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_FederalStates_FederalStateId",
                        column: x => x.FederalStateId,
                        principalSchema: "de",
                        principalTable: "FederalStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Districts_GovernmentRegions_GovernmentRegionId",
                        column: x => x.GovernmentRegionId,
                        principalSchema: "de",
                        principalTable: "GovernmentRegions",
                        principalColumn: "Id");
                },
                comment: "Representation of a German district (Kreis)");

            migrationBuilder.CreateTable(
                name: "FullTextStreets",
                schema: "ch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Straßenschlüssel)"),
                    Locality = table.Column<string>(type: "text", nullable: false, comment: "Locality (Ort)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Straßenname)"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, comment: "Postal code (Postleitzahl)"),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true, comment: "tsvector column for full text search")
                        .Annotation("Npgsql:TsVectorConfig", "config_openplzapi")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "PostalCode", "Locality" }),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Status (Straßenstatus)"),
                    CommuneId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to commune (Gemeinde)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullTextStreets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FullTextStreets_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalSchema: "ch",
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a Swiss street (Straße) for full text search");

            migrationBuilder.CreateTable(
                name: "Localities",
                schema: "ch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Ortsname)"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, comment: "Postal code (Postleitzahl)"),
                    CommuneId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to commune (Gemeinde)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localities_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalSchema: "ch",
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a Swiss locality (Ort oder Stadt)");

            migrationBuilder.CreateTable(
                name: "FullTextStreets",
                schema: "at",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Straßenkennziffer)"),
                    Locality = table.Column<string>(type: "text", nullable: false, comment: "Locality (Ortschaft)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Straßenname)"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, comment: "Postal code (Postleitzahl)"),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true, comment: "tsvector column for full text search")
                        .Annotation("Npgsql:TsVectorConfig", "config_openplzapi")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "PostalCode", "Locality" }),
                    MunicipalityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to municipality")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullTextStreets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FullTextStreets_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "at",
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of an Austrian street (Straße) for full text search");

            migrationBuilder.CreateTable(
                name: "Localities",
                schema: "at",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Ortschaftskennziffer)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Ortschaftsname)"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, comment: "Postal code (Postleitzahl)"),
                    MunicipalityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to municipality")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localities_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "at",
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of an Austrian locality (Stadt, Ort)");

            migrationBuilder.CreateTable(
                name: "MunicipalAssociations",
                schema: "de",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    AdministrativeHeadquarters = table.Column<string>(type: "text", nullable: true, comment: "Administrative headquarters (Verwaltungssitz des Gemeindeverbandes)"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Regional key (Regionalschlüssel)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Name des Gemeindeverbandes)"),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Type (Kennzeichen)"),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Reference to district (Kreis)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalAssociations_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "de",
                        principalTable: "Districts",
                        principalColumn: "Id");
                },
                comment: "Representation of a German municipal association (Gemeindeverband)");

            migrationBuilder.CreateTable(
                name: "Streets",
                schema: "ch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Straßenschlüssel)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Straßenname)"),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Status (Straßenstatus)"),
                    LocalityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to locality")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streets_Localities_LocalityId",
                        column: x => x.LocalityId,
                        principalSchema: "ch",
                        principalTable: "Localities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a Swiss street (Straße)");

            migrationBuilder.CreateTable(
                name: "Streets",
                schema: "at",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Key (Straßenkennziffer)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Straßenname)"),
                    LocalityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to locality")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streets_Localities_LocalityId",
                        column: x => x.LocalityId,
                        principalSchema: "at",
                        principalTable: "Localities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of an Austrian street (Straße)");

            migrationBuilder.CreateTable(
                name: "Municipalities",
                schema: "de",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "Regional key (Regionalschlüssel)"),
                    MultiplePostalCodes = table.Column<bool>(type: "boolean", nullable: false, comment: "Multiple postcodes available?"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Gemeindename)"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, comment: "Postal code of the administrative headquarters (Verwaltungssitz), if there are multiple postal codes available"),
                    ShortName = table.Column<string>(type: "text", nullable: false, comment: "Short Name (Verkürzter Gemeindename)"),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Type (Gemeindekennzeichen)"),
                    AssociationId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Reference to municipal association (Gemeindeverband)"),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Reference to district (Kreis)"),
                    FederalStateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to federal state (Bundesland)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipalities_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "de",
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Municipalities_FederalStates_FederalStateId",
                        column: x => x.FederalStateId,
                        principalSchema: "de",
                        principalTable: "FederalStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Municipalities_MunicipalAssociations_AssociationId",
                        column: x => x.AssociationId,
                        principalSchema: "de",
                        principalTable: "MunicipalAssociations",
                        principalColumn: "Id");
                },
                comment: "Representation of a German municipality (Gemeinde)");

            migrationBuilder.CreateTable(
                name: "FullTextStreets",
                schema: "de",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Borough = table.Column<string>(type: "text", nullable: true, comment: "Borough (Stadtbezirk)"),
                    Locality = table.Column<string>(type: "text", nullable: false, comment: "Locality (Ort)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Straßenname)"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, comment: "Postal code (Postleitzahl)"),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true, comment: "tsvector column for full text search")
                        .Annotation("Npgsql:TsVectorConfig", "config_openplzapi")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "PostalCode", "Locality" }),
                    Suburb = table.Column<string>(type: "text", nullable: true, comment: "Suburb (Orts- oder Stadtteil)"),
                    MunicipalityId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Reference to municipality (Gemeinde)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullTextStreets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FullTextStreets_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "de",
                        principalTable: "Municipalities",
                        principalColumn: "Id");
                },
                comment: "Representation of a German street (Straße) for full text search");

            migrationBuilder.CreateTable(
                name: "Localities",
                schema: "de",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Ortsname)"),
                    PostalCode = table.Column<string>(type: "text", nullable: false, comment: "Postal code (Postleitzahl)"),
                    MunicipalityId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Reference to municipality (Gemeinde)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localities_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalSchema: "de",
                        principalTable: "Municipalities",
                        principalColumn: "Id");
                },
                comment: "Representation of a German locality (Ort oder Stadt)");

            migrationBuilder.CreateTable(
                name: "Streets",
                schema: "de",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Borough = table.Column<string>(type: "text", nullable: true, comment: "Borough (Stadtbezirk)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Name (Straßenname)"),
                    Suburb = table.Column<string>(type: "text", nullable: true, comment: "Suburb (Orts- oder Stadtteil)"),
                    LocalityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to locality")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streets_Localities_LocalityId",
                        column: x => x.LocalityId,
                        principalSchema: "de",
                        principalTable: "Localities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a German street (Straße)");

            migrationBuilder.CreateIndex(
                name: "IX_Cantons_Key",
                schema: "ch",
                table: "Cantons",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communes_DistrictId",
                schema: "ch",
                table: "Communes",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Communes_Key",
                schema: "ch",
                table: "Communes",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communes_Key1",
                schema: "li",
                table: "Communes",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Code",
                schema: "at",
                table: "Districts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_FederalProvinceId",
                schema: "at",
                table: "Districts",
                column: "FederalProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CantonId",
                schema: "ch",
                table: "Districts",
                column: "CantonId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Key",
                schema: "ch",
                table: "Districts",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_FederalStateId",
                schema: "de",
                table: "Districts",
                column: "FederalStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_GovernmentRegionId",
                schema: "de",
                table: "Districts",
                column: "GovernmentRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Key1",
                schema: "de",
                table: "Districts",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FederalProvinces_Key",
                schema: "at",
                table: "FederalProvinces",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FederalStates_Key",
                schema: "de",
                table: "FederalStates",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FullTextStreets_MunicipalityId",
                schema: "at",
                table: "FullTextStreets",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_FullTextStreets_SearchVector",
                schema: "at",
                table: "FullTextStreets",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_FullTextStreets_CommuneId",
                schema: "ch",
                table: "FullTextStreets",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_FullTextStreets_SearchVector1",
                schema: "ch",
                table: "FullTextStreets",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_FullTextStreets_MunicipalityId1",
                schema: "de",
                table: "FullTextStreets",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_FullTextStreets_SearchVector2",
                schema: "de",
                table: "FullTextStreets",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_FullTextStreets_CommuneId1",
                schema: "li",
                table: "FullTextStreets",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_FullTextStreets_SearchVector3",
                schema: "li",
                table: "FullTextStreets",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentRegions_FederalStateId",
                schema: "de",
                table: "GovernmentRegions",
                column: "FederalStateId");

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentRegions_Key",
                schema: "de",
                table: "GovernmentRegions",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localities_Key_PostalCode_MunicipalityId",
                schema: "at",
                table: "Localities",
                columns: new[] { "Key", "PostalCode", "MunicipalityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localities_MunicipalityId",
                schema: "at",
                table: "Localities",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Localities_CommuneId_PostalCode_Name",
                schema: "ch",
                table: "Localities",
                columns: new[] { "CommuneId", "PostalCode", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localities_MunicipalityId_PostalCode_Name",
                schema: "de",
                table: "Localities",
                columns: new[] { "MunicipalityId", "PostalCode", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localities_CommuneId_PostalCode_Name1",
                schema: "li",
                table: "Localities",
                columns: new[] { "CommuneId", "PostalCode", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalAssociations_DistrictId",
                schema: "de",
                table: "MunicipalAssociations",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalAssociations_Key",
                schema: "de",
                table: "MunicipalAssociations",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_Code",
                schema: "at",
                table: "Municipalities",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_DistrictId",
                schema: "at",
                table: "Municipalities",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_AssociationId",
                schema: "de",
                table: "Municipalities",
                column: "AssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_DistrictId1",
                schema: "de",
                table: "Municipalities",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_FederalStateId",
                schema: "de",
                table: "Municipalities",
                column: "FederalStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_Key",
                schema: "de",
                table: "Municipalities",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Streets_Key_LocalityId",
                schema: "at",
                table: "Streets",
                columns: new[] { "Key", "LocalityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Streets_LocalityId",
                schema: "at",
                table: "Streets",
                column: "LocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_Key_LocalityId1",
                schema: "ch",
                table: "Streets",
                columns: new[] { "Key", "LocalityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Streets_LocalityId1",
                schema: "ch",
                table: "Streets",
                column: "LocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_LocalityId2",
                schema: "de",
                table: "Streets",
                column: "LocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_Name_LocalityId",
                schema: "de",
                table: "Streets",
                columns: new[] { "Name", "LocalityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Streets_Key_LocalityId2",
                schema: "li",
                table: "Streets",
                columns: new[] { "Key", "LocalityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Streets_LocalityId3",
                schema: "li",
                table: "Streets",
                column: "LocalityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FullTextStreets",
                schema: "at");

            migrationBuilder.DropTable(
                name: "FullTextStreets",
                schema: "ch");

            migrationBuilder.DropTable(
                name: "FullTextStreets",
                schema: "de");

            migrationBuilder.DropTable(
                name: "FullTextStreets",
                schema: "li");

            migrationBuilder.DropTable(
                name: "Streets",
                schema: "at");

            migrationBuilder.DropTable(
                name: "Streets",
                schema: "ch");

            migrationBuilder.DropTable(
                name: "Streets",
                schema: "de");

            migrationBuilder.DropTable(
                name: "Streets",
                schema: "li");

            migrationBuilder.DropTable(
                name: "Localities",
                schema: "at");

            migrationBuilder.DropTable(
                name: "Localities",
                schema: "ch");

            migrationBuilder.DropTable(
                name: "Localities",
                schema: "de");

            migrationBuilder.DropTable(
                name: "Localities",
                schema: "li");

            migrationBuilder.DropTable(
                name: "Municipalities",
                schema: "at");

            migrationBuilder.DropTable(
                name: "Communes",
                schema: "ch");

            migrationBuilder.DropTable(
                name: "Municipalities",
                schema: "de");

            migrationBuilder.DropTable(
                name: "Communes",
                schema: "li");

            migrationBuilder.DropTable(
                name: "Districts",
                schema: "at");

            migrationBuilder.DropTable(
                name: "Districts",
                schema: "ch");

            migrationBuilder.DropTable(
                name: "MunicipalAssociations",
                schema: "de");

            migrationBuilder.DropTable(
                name: "FederalProvinces",
                schema: "at");

            migrationBuilder.DropTable(
                name: "Cantons",
                schema: "ch");

            migrationBuilder.DropTable(
                name: "Districts",
                schema: "de");

            migrationBuilder.DropTable(
                name: "GovernmentRegions",
                schema: "de");

            migrationBuilder.DropTable(
                name: "FederalStates",
                schema: "de");
        }
    }
}
