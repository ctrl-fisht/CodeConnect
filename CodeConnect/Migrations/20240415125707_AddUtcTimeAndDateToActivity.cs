using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddUtcTimeAndDateToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocalTime",
                table: "Activities",
                newName: "TimeUtc");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Activities",
                newName: "DateUtc");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateLocal",
                table: "Activities",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeLocal",
                table: "Activities",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateLocal",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "TimeLocal",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "TimeUtc",
                table: "Activities",
                newName: "LocalTime");

            migrationBuilder.RenameColumn(
                name: "DateUtc",
                table: "Activities",
                newName: "Date");
        }
    }
}
