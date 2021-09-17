using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamCar.DAL.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImported",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "Power",
                table: "Cars",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImported",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Cars");
        }
    }
}
