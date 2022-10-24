using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class UpUserIdInFolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "WebFolders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WebFolders_CreatorId",
                table: "WebFolders",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebFolders_AspNetUsers_CreatorId",
                table: "WebFolders",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebFolders_AspNetUsers_CreatorId",
                table: "WebFolders");

            migrationBuilder.DropIndex(
                name: "IX_WebFolders_CreatorId",
                table: "WebFolders");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "WebFolders");
        }
    }
}
