using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Communities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Activities");
        }
    }
}
