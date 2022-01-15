using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamCar.DAL.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowAdverts_Adverts_AdvertId",
                table: "FollowAdverts");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowAdverts_Adverts_AdvertId",
                table: "FollowAdverts",
                column: "AdvertId",
                principalTable: "Adverts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowAdverts_Adverts_AdvertId",
                table: "FollowAdverts");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowAdverts_Adverts_AdvertId",
                table: "FollowAdverts",
                column: "AdvertId",
                principalTable: "Adverts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
