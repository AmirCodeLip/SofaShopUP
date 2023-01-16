using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Access.Migrations
{
    public partial class TypeKind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "TypeKind",
                table: "WebFiles",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeKind",
                table: "WebFiles");
        }
    }
}
