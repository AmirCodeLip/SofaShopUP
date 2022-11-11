using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class WebActorOrArtistRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebActorOrArtist_WebActorOrArtist_ParentID",
                table: "WebActorOrArtist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebActorOrArtist",
                table: "WebActorOrArtist");

            migrationBuilder.RenameTable(
                name: "WebActorOrArtist",
                newName: "WebActorOrArtists");

            migrationBuilder.RenameIndex(
                name: "IX_WebActorOrArtist_ParentID",
                table: "WebActorOrArtists",
                newName: "IX_WebActorOrArtists_ParentID");

            migrationBuilder.AddColumn<Guid>(
                name: "WebFileVersionId",
                table: "WebActorOrArtists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebActorOrArtists",
                table: "WebActorOrArtists",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WebActorOrArtists_WebFileVersionId",
                table: "WebActorOrArtists",
                column: "WebFileVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebActorOrArtists_WebActorOrArtists_ParentID",
                table: "WebActorOrArtists",
                column: "ParentID",
                principalTable: "WebActorOrArtists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebActorOrArtists_WebFileVersions_WebFileVersionId",
                table: "WebActorOrArtists",
                column: "WebFileVersionId",
                principalTable: "WebFileVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebActorOrArtists_WebActorOrArtists_ParentID",
                table: "WebActorOrArtists");

            migrationBuilder.DropForeignKey(
                name: "FK_WebActorOrArtists_WebFileVersions_WebFileVersionId",
                table: "WebActorOrArtists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebActorOrArtists",
                table: "WebActorOrArtists");

            migrationBuilder.DropIndex(
                name: "IX_WebActorOrArtists_WebFileVersionId",
                table: "WebActorOrArtists");

            migrationBuilder.DropColumn(
                name: "WebFileVersionId",
                table: "WebActorOrArtists");

            migrationBuilder.RenameTable(
                name: "WebActorOrArtists",
                newName: "WebActorOrArtist");

            migrationBuilder.RenameIndex(
                name: "IX_WebActorOrArtists_ParentID",
                table: "WebActorOrArtist",
                newName: "IX_WebActorOrArtist_ParentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebActorOrArtist",
                table: "WebActorOrArtist",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WebActorOrArtist_WebActorOrArtist_ParentID",
                table: "WebActorOrArtist",
                column: "ParentID",
                principalTable: "WebActorOrArtist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
