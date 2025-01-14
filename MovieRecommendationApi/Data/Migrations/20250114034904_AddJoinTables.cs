using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRecommendationApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJoinTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionCompanies_Movies_MovieId",
                table: "ProductionCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionCountries_Movies_MovieId",
                table: "ProductionCountries");

            migrationBuilder.DropIndex(
                name: "IX_ProductionCountries_MovieId",
                table: "ProductionCountries");

            migrationBuilder.DropIndex(
                name: "IX_ProductionCompanies_MovieId",
                table: "ProductionCompanies");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "ProductionCountries");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "ProductionCompanies");

            migrationBuilder.CreateTable(
                name: "MovieProductionCompany",
                columns: table => new
                {
                    MoviesId = table.Column<string>(type: "text", nullable: false),
                    ProductionCompaniesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieProductionCompany", x => new { x.MoviesId, x.ProductionCompaniesId });
                    table.ForeignKey(
                        name: "FK_MovieProductionCompany_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieProductionCompany_ProductionCompanies_ProductionCompan~",
                        column: x => x.ProductionCompaniesId,
                        principalTable: "ProductionCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieProductionCountry",
                columns: table => new
                {
                    MoviesId = table.Column<string>(type: "text", nullable: false),
                    ProductionCountriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieProductionCountry", x => new { x.MoviesId, x.ProductionCountriesId });
                    table.ForeignKey(
                        name: "FK_MovieProductionCountry_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieProductionCountry_ProductionCountries_ProductionCountr~",
                        column: x => x.ProductionCountriesId,
                        principalTable: "ProductionCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCompany_ProductionCompaniesId",
                table: "MovieProductionCompany",
                column: "ProductionCompaniesId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCountry_ProductionCountriesId",
                table: "MovieProductionCountry",
                column: "ProductionCountriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieProductionCompany");

            migrationBuilder.DropTable(
                name: "MovieProductionCountry");

            migrationBuilder.AddColumn<string>(
                name: "MovieId",
                table: "ProductionCountries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MovieId",
                table: "ProductionCompanies",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionCountries_MovieId",
                table: "ProductionCountries",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionCompanies_MovieId",
                table: "ProductionCompanies",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionCompanies_Movies_MovieId",
                table: "ProductionCompanies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionCountries_Movies_MovieId",
                table: "ProductionCountries",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
