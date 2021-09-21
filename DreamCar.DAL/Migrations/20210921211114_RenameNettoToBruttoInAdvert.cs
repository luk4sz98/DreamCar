using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamCar.DAL.Migrations
{
    public partial class RenameNettoToBruttoInAdvert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Netto",
                table: "Adverts",
                newName: "Brutto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Brutto",
                table: "Adverts",
                newName: "Netto");
        }
    }
}
