using Microsoft.EntityFrameworkCore.Migrations;

namespace VkWeb.Migrations
{
    public partial class StudentLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentLimit",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentLimit",
                table: "Organizations");
        }
    }
}
