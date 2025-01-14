using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRecommendationApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditMovieTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Credits_CreditsId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "CreditsId",
                table: "Movies",
                newName: "CreditId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_CreditsId",
                table: "Movies",
                newName: "IX_Movies_CreditId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Credits_CreditId",
                table: "Movies",
                column: "CreditId",
                principalTable: "Credits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Credits_CreditId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "CreditId",
                table: "Movies",
                newName: "CreditsId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_CreditId",
                table: "Movies",
                newName: "IX_Movies_CreditsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Credits_CreditsId",
                table: "Movies",
                column: "CreditsId",
                principalTable: "Credits",
                principalColumn: "Id");
        }
    }
}
