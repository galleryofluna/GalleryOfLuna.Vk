using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GalleryOfLuna.Vk.Publishing.EntityFramework.Sqlite.Migrations
{
    public partial class AddSkippedProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedOn",
                table: "PublishedImages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<bool>(
                name: "Skipped",
                table: "PublishedImages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skipped",
                table: "PublishedImages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedOn",
                table: "PublishedImages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
