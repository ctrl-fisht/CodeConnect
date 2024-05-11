using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeConnect.Migrations
{
    /// <inheritdoc />
    public partial class ExtendCommunity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Communities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Communities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramTag",
                table: "Communities",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "TelegramTag",
                table: "Communities");
        }
    }
}
