using Microsoft.EntityFrameworkCore.Migrations;

namespace SYT.VendingMachineSystem.Migrations
{
    public partial class Added_Restart_to_VendingMachineEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Restart",
                table: "VendingMachines",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Restart",
                table: "VendingMachines");
        }
    }
}
