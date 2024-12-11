using Microsoft.EntityFrameworkCore.Migrations;

namespace VkWeb.Migrations
{
    public partial class OrgReading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgReading_Organizations_OrgId",
                table: "OrgReading");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgReading_Readings_ReadingId",
                table: "OrgReading");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgReading_Users_VkUserId",
                table: "OrgReading");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgReading",
                table: "OrgReading");

            migrationBuilder.RenameTable(
                name: "OrgReading",
                newName: "OrgReadings");

            migrationBuilder.RenameIndex(
                name: "IX_OrgReading_VkUserId",
                table: "OrgReadings",
                newName: "IX_OrgReadings_VkUserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrgReading_ReadingId",
                table: "OrgReadings",
                newName: "IX_OrgReadings_ReadingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgReadings",
                table: "OrgReadings",
                columns: new[] { "OrgId", "ReadingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrgReadings_Organizations_OrgId",
                table: "OrgReadings",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgReadings_Readings_ReadingId",
                table: "OrgReadings",
                column: "ReadingId",
                principalTable: "Readings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgReadings_Users_VkUserId",
                table: "OrgReadings",
                column: "VkUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgReadings_Organizations_OrgId",
                table: "OrgReadings");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgReadings_Readings_ReadingId",
                table: "OrgReadings");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgReadings_Users_VkUserId",
                table: "OrgReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgReadings",
                table: "OrgReadings");

            migrationBuilder.RenameTable(
                name: "OrgReadings",
                newName: "OrgReading");

            migrationBuilder.RenameIndex(
                name: "IX_OrgReadings_VkUserId",
                table: "OrgReading",
                newName: "IX_OrgReading_VkUserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrgReadings_ReadingId",
                table: "OrgReading",
                newName: "IX_OrgReading_ReadingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgReading",
                table: "OrgReading",
                columns: new[] { "OrgId", "ReadingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrgReading_Organizations_OrgId",
                table: "OrgReading",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgReading_Readings_ReadingId",
                table: "OrgReading",
                column: "ReadingId",
                principalTable: "Readings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgReading_Users_VkUserId",
                table: "OrgReading",
                column: "VkUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
