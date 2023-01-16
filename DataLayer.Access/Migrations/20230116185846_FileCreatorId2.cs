using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class FileCreatorId2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebFiles_AspNetUsers_WebUserId",
                table: "WebFiles");

            migrationBuilder.DropIndex(
                name: "IX_WebFiles_WebUserId",
                table: "WebFiles");

            migrationBuilder.DropColumn(
                name: "WebUserId",
                table: "WebFiles");

            migrationBuilder.CreateIndex(
                name: "IX_WebFiles_CreatorId",
                table: "WebFiles",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebFiles_AspNetUsers_CreatorId",
                table: "WebFiles",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebFiles_AspNetUsers_CreatorId",
                table: "WebFiles");

            migrationBuilder.DropIndex(
                name: "IX_WebFiles_CreatorId",
                table: "WebFiles");

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
    }
}
