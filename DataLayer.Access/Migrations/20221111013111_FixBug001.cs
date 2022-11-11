using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class FixBug001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebActorOrArtists_WebFileVersions_WebFileVersionId",
                table: "WebActorOrArtists");

            migrationBuilder.DropIndex(
                name: "IX_WebActorOrArtists_WebFileVersionId",
                table: "WebActorOrArtists");

            migrationBuilder.DropColumn(
                name: "WebFileVersionId",
                table: "WebActorOrArtists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WebFileVersionId",
                table: "WebActorOrArtists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WebActorOrArtists_WebFileVersionId",
                table: "WebActorOrArtists",
                column: "WebFileVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebActorOrArtists_WebFileVersions_WebFileVersionId",
                table: "WebActorOrArtists",
                column: "WebFileVersionId",
                principalTable: "WebFileVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
