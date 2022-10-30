using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class FolderIdRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebFiles_WebFolders_FolderId",
                table: "WebFiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "FolderId",
                table: "WebFiles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_WebFiles_WebFolders_FolderId",
                table: "WebFiles",
                column: "FolderId",
                principalTable: "WebFolders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebFiles_WebFolders_FolderId",
                table: "WebFiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "FolderId",
                table: "WebFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WebFiles_WebFolders_FolderId",
                table: "WebFiles",
                column: "FolderId",
                principalTable: "WebFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
