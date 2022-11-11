using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class FixUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebActorOrArtists_WebActorOrArtists_ParentID",
                table: "WebActorOrArtists");

            migrationBuilder.RenameColumn(
                name: "ParentID",
                table: "WebActorOrArtists",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_WebActorOrArtists_ParentID",
                table: "WebActorOrArtists",
                newName: "IX_WebActorOrArtists_ParentId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WebActorOrArtists",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Culture",
                table: "WebActorOrArtists",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "WebFileVersionActorOrArtists",
                columns: table => new
                {
                    WebActorOrArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebFileVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebFileVersionActorOrArtists", x => new { x.WebFileVersionId, x.WebActorOrArtistId });
                    table.ForeignKey(
                        name: "FK_WebFileVersionActorOrArtists_WebActorOrArtists_WebActorOrArtistId",
                        column: x => x.WebActorOrArtistId,
                        principalTable: "WebActorOrArtists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebFileVersionActorOrArtists_WebFileVersions_WebFileVersionId",
                        column: x => x.WebFileVersionId,
                        principalTable: "WebFileVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebActorOrArtists_Name_Culture",
                table: "WebActorOrArtists",
                columns: new[] { "Name", "Culture" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Culture] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WebFileVersionActorOrArtists_WebActorOrArtistId",
                table: "WebFileVersionActorOrArtists",
                column: "WebActorOrArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebActorOrArtists_WebActorOrArtists_ParentId",
                table: "WebActorOrArtists",
                column: "ParentId",
                principalTable: "WebActorOrArtists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebActorOrArtists_WebActorOrArtists_ParentId",
                table: "WebActorOrArtists");

            migrationBuilder.DropTable(
                name: "WebFileVersionActorOrArtists");

            migrationBuilder.DropIndex(
                name: "IX_WebActorOrArtists_Name_Culture",
                table: "WebActorOrArtists");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "WebActorOrArtists",
                newName: "ParentID");

            migrationBuilder.RenameIndex(
                name: "IX_WebActorOrArtists_ParentId",
                table: "WebActorOrArtists",
                newName: "IX_WebActorOrArtists_ParentID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WebActorOrArtists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Culture",
                table: "WebActorOrArtists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WebActorOrArtists_WebActorOrArtists_ParentID",
                table: "WebActorOrArtists",
                column: "ParentID",
                principalTable: "WebActorOrArtists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
