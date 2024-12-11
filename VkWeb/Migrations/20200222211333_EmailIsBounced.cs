using Microsoft.EntityFrameworkCore.Migrations;

namespace VkWeb.Migrations
{
    public partial class EmailIsBounced : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgAdmin_Organizations_OrgId",
                table: "OrgAdmin");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgAdmin_Users_VkUserId",
                table: "OrgAdmin");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgStudent_Organizations_OrgId",
                table: "OrgStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgStudent_Users_VkUserId",
                table: "OrgStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgStudent",
                table: "OrgStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgAdmin",
                table: "OrgAdmin");

            migrationBuilder.RenameTable(
                name: "OrgStudent",
                newName: "OrgStudents");

            migrationBuilder.RenameTable(
                name: "OrgAdmin",
                newName: "OrgAdmins");

            migrationBuilder.RenameIndex(
                name: "IX_OrgStudent_VkUserId",
                table: "OrgStudents",
                newName: "IX_OrgStudents_VkUserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrgAdmin_VkUserId",
                table: "OrgAdmins",
                newName: "IX_OrgAdmins_VkUserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsBounced",
                table: "Invites",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgStudents",
                table: "OrgStudents",
                columns: new[] { "OrgId", "VkUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgAdmins",
                table: "OrgAdmins",
                columns: new[] { "OrgId", "VkUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrgAdmins_Organizations_OrgId",
                table: "OrgAdmins",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgAdmins_Users_VkUserId",
                table: "OrgAdmins",
                column: "VkUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgStudents_Organizations_OrgId",
                table: "OrgStudents",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgStudents_Users_VkUserId",
                table: "OrgStudents",
                column: "VkUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgAdmins_Organizations_OrgId",
                table: "OrgAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgAdmins_Users_VkUserId",
                table: "OrgAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgStudents_Organizations_OrgId",
                table: "OrgStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgStudents_Users_VkUserId",
                table: "OrgStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgStudents",
                table: "OrgStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgAdmins",
                table: "OrgAdmins");

            migrationBuilder.DropColumn(
                name: "IsBounced",
                table: "Invites");

            migrationBuilder.RenameTable(
                name: "OrgStudents",
                newName: "OrgStudent");

            migrationBuilder.RenameTable(
                name: "OrgAdmins",
                newName: "OrgAdmin");

            migrationBuilder.RenameIndex(
                name: "IX_OrgStudents_VkUserId",
                table: "OrgStudent",
                newName: "IX_OrgStudent_VkUserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrgAdmins_VkUserId",
                table: "OrgAdmin",
                newName: "IX_OrgAdmin_VkUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgStudent",
                table: "OrgStudent",
                columns: new[] { "OrgId", "VkUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgAdmin",
                table: "OrgAdmin",
                columns: new[] { "OrgId", "VkUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrgAdmin_Organizations_OrgId",
                table: "OrgAdmin",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgAdmin_Users_VkUserId",
                table: "OrgAdmin",
                column: "VkUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgStudent_Organizations_OrgId",
                table: "OrgStudent",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgStudent_Users_VkUserId",
                table: "OrgStudent",
                column: "VkUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
