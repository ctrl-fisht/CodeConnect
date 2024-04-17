using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeConnect.Migrations
{
    /// <inheritdoc />
    public partial class FixUserGuidToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityUsers_AspNetUsers_UserId1",
                table: "ActivityUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityUsers_AspNetUsers_UserId1",
                table: "CommunityUsers");

            migrationBuilder.DropIndex(
                name: "IX_CommunityUsers_UserId1",
                table: "CommunityUsers");

            migrationBuilder.DropIndex(
                name: "IX_ActivityUsers_UserId1",
                table: "ActivityUsers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "CommunityUsers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ActivityUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommunityUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ActivityUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityUsers_UserId",
                table: "CommunityUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityUsers_UserId",
                table: "ActivityUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityUsers_AspNetUsers_UserId",
                table: "ActivityUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityUsers_AspNetUsers_UserId",
                table: "CommunityUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityUsers_AspNetUsers_UserId",
                table: "ActivityUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityUsers_AspNetUsers_UserId",
                table: "CommunityUsers");

            migrationBuilder.DropIndex(
                name: "IX_CommunityUsers_UserId",
                table: "CommunityUsers");

            migrationBuilder.DropIndex(
                name: "IX_ActivityUsers_UserId",
                table: "ActivityUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CommunityUsers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "CommunityUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ActivityUsers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ActivityUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityUsers_UserId1",
                table: "CommunityUsers",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityUsers_UserId1",
                table: "ActivityUsers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityUsers_AspNetUsers_UserId1",
                table: "ActivityUsers",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityUsers_AspNetUsers_UserId1",
                table: "CommunityUsers",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
