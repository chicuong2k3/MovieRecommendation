using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRecommendationApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_WatchMovies_Users_UserId",
                table: "WatchMovies");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddForeignKey(
                name: "FK_WatchMovies_Users_UserId",
                table: "WatchMovies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
