using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRecommendationApi.Migrations
{
    /// <inheritdoc />
    public partial class mg2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdMovie",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SpokenLanguages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_IdMovie",
                table: "Videos",
                column: "IdMovie");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Movie_IdMovie",
                table: "Videos",
                column: "IdMovie",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Movie_IdMovie",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_IdMovie",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "IdMovie",
                table: "Videos");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SpokenLanguages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
