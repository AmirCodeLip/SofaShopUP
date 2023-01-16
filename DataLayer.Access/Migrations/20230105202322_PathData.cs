using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class PathData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "WebFolders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "WebFolders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "WebFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "WebFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "WebFolders");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "WebFolders");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "WebFiles");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "WebFiles");
        }
    }
}
