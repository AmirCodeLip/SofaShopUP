using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class AddFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileDataPath",
                table: "WebFileVersions");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "WebFileVersions",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "WebFileVersions");

            migrationBuilder.AddColumn<string>(
                name: "FileDataPath",
                table: "WebFileVersions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
