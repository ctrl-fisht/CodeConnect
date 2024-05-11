using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeConnect.Migrations
{
    /// <inheritdoc />
    public partial class ExtendActivityModeration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Declined",
                table: "Activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Declined",
                table: "Activities");
        }
    }
}
