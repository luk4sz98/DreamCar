using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamCar.DAL.Migrations
{
    public partial class ChangedDeleteBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarEquipments_Cars_CarId",
                table: "CarEquipments");

            migrationBuilder.DropForeignKey(
                name: "FK_CarEquipments_Equipment_EquipmentId",
                table: "CarEquipments");

            migrationBuilder.AddForeignKey(
                name: "FK_CarEquipments_Cars_CarId",
                table: "CarEquipments",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarEquipments_Equipment_EquipmentId",
                table: "CarEquipments",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarEquipments_Cars_CarId",
                table: "CarEquipments");

            migrationBuilder.DropForeignKey(
                name: "FK_CarEquipments_Equipment_EquipmentId",
                table: "CarEquipments");

            migrationBuilder.AddForeignKey(
                name: "FK_CarEquipments_Cars_CarId",
                table: "CarEquipments",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarEquipments_Equipment_EquipmentId",
                table: "CarEquipments",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
