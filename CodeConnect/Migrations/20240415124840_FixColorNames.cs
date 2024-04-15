using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeConnect.Migrations
{
    /// <inheritdoc />
    public partial class FixColorNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorString",
                table: "Tags",
                newName: "ColorHex");

            migrationBuilder.RenameColumn(
                name: "ColorString",
                table: "Categories",
                newName: "ColorHex");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorHex",
                table: "Tags",
                newName: "ColorString");

            migrationBuilder.RenameColumn(
                name: "ColorHex",
                table: "Categories",
                newName: "ColorString");
        }
    }
}
