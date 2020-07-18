using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class sharetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Post",
                table: "Share");

            migrationBuilder.DropColumn(
                name: "ShareById",
                table: "Share");

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Share",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Share",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Share");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Share");

            migrationBuilder.AddColumn<string>(
                name: "Post",
                table: "Share",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShareById",
                table: "Share",
                nullable: false,
                defaultValue: 0);
        }
    }
}
