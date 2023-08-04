using Microsoft.EntityFrameworkCore.Migrations;

namespace VeterinaryClinic.Migrations
{
    public partial class ChangeRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_UserId",
                table: "Rooms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_UserId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rooms");
        }
    }
}
