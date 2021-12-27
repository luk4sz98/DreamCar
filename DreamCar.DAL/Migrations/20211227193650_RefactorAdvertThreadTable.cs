using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamCar.DAL.Migrations
{
    public partial class RefactorAdvertThreadTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AdvertThreads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "AdvertThreads",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AdvertThreads");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AdvertThreads");
        }
    }
}
