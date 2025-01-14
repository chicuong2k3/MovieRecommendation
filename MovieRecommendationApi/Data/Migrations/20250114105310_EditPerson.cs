using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRecommendationApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_MovieCredits_MovieCreditsId",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "MovieCreditsId",
                table: "People",
                newName: "MovieCreditId");

            migrationBuilder.RenameIndex(
                name: "IX_People_MovieCreditsId",
                table: "People",
                newName: "IX_People_MovieCreditId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_MovieCredits_MovieCreditId",
                table: "People",
                column: "MovieCreditId",
                principalTable: "MovieCredits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_MovieCredits_MovieCreditId",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "MovieCreditId",
                table: "People",
                newName: "MovieCreditsId");

            migrationBuilder.RenameIndex(
                name: "IX_People_MovieCreditId",
                table: "People",
                newName: "IX_People_MovieCreditsId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_MovieCredits_MovieCreditsId",
                table: "People",
                column: "MovieCreditsId",
                principalTable: "MovieCredits",
                principalColumn: "Id");
        }
    }
}
