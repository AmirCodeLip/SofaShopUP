using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class AddInfoData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "WebFileVersions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "WebFileVersions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebFileVersions_ParentId",
                table: "WebFileVersions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebFileVersions_WebFileVersions_ParentId",
                table: "WebFileVersions",
                column: "ParentId",
                principalTable: "WebFileVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebFileVersions_WebFileVersions_ParentId",
                table: "WebFileVersions");

            migrationBuilder.DropIndex(
                name: "IX_WebFileVersions_ParentId",
                table: "WebFileVersions");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "WebFileVersions");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "WebFileVersions");
        }
    }
}
