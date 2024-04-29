using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeConnect.Migrations
{
    /// <inheritdoc />
    public partial class RenameNotificationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sent4",
                table: "Notifications",
                newName: "SentSecond");

            migrationBuilder.RenameColumn(
                name: "Sent24",
                table: "Notifications",
                newName: "SentFirst");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentSecond",
                table: "Notifications",
                newName: "Sent4");

            migrationBuilder.RenameColumn(
                name: "SentFirst",
                table: "Notifications",
                newName: "Sent24");
        }
    }
}
