using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamCar.DAL.Migrations
{
    public partial class UpdateAdvertCarMessageTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Messages",
                newName: "Content");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FirstRegistration",
                table: "Cars",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedAt",
                table: "Adverts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Adverts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Adverts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Adverts");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Messages",
                newName: "Description");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FirstRegistration",
                table: "Cars",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
