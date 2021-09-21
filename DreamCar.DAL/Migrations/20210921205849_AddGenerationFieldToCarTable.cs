using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamCar.DAL.Migrations
{
    public partial class AddGenerationFieldToCarTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Generation",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Generation",
                table: "Cars");
        }
    }
}
