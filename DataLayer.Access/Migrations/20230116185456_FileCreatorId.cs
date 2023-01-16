using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class FileCreatorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "WebFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WebUserId",
                table: "WebFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebFiles_WebUserId",
                table: "WebFiles",
                column: "WebUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebFiles_AspNetUsers_WebUserId",
                table: "WebFiles",
                column: "WebUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebFiles_AspNetUsers_WebUserId",
                table: "WebFiles");

            migrationBuilder.DropIndex(
                name: "IX_WebFiles_WebUserId",
                table: "WebFiles");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "WebFiles");

            migrationBuilder.DropColumn(
                name: "WebUserId",
                table: "WebFiles");
        }
    }
}
