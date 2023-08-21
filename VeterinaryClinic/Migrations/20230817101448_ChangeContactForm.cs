using Microsoft.EntityFrameworkCore.Migrations;

namespace VeterinaryClinic.Migrations
{
    public partial class ChangeContactForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Contacts",
                newName: "Subject");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Contacts",
                newName: "Phone");
        }
    }
}
