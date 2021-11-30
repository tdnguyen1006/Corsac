using Microsoft.EntityFrameworkCore.Migrations;

namespace Corsac.Data.Migrations
{
    public partial class AddUIDToTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UID",
                table: "Ticket",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UID",
                table: "Ticket");
        }
    }
}
