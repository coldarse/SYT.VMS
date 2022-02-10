using Microsoft.EntityFrameworkCore.Migrations;

namespace SYT.VendingMachineSystem.Migrations
{
    public partial class Added_ItemCode_to_ActivityLogEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                table: "ActivityLogs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCode",
                table: "ActivityLogs");
        }
    }
}
