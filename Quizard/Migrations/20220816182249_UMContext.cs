using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class UMContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserModule_AspNetUsers_UserId",
                table: "UserModule");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModule_Modules_ModuleId",
                table: "UserModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModule",
                table: "UserModule");

            migrationBuilder.RenameTable(
                name: "UserModule",
                newName: "UserModules");

            migrationBuilder.RenameIndex(
                name: "IX_UserModule_ModuleId",
                table: "UserModules",
                newName: "IX_UserModules_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModules",
                table: "UserModules",
                columns: new[] { "UserId", "ModuleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserModules_AspNetUsers_UserId",
                table: "UserModules",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModules_Modules_ModuleId",
                table: "UserModules",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserModules_AspNetUsers_UserId",
                table: "UserModules");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModules_Modules_ModuleId",
                table: "UserModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModules",
                table: "UserModules");

            migrationBuilder.RenameTable(
                name: "UserModules",
                newName: "UserModule");

            migrationBuilder.RenameIndex(
                name: "IX_UserModules_ModuleId",
                table: "UserModule",
                newName: "IX_UserModule_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModule",
                table: "UserModule",
                columns: new[] { "UserId", "ModuleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserModule_AspNetUsers_UserId",
                table: "UserModule",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModule_Modules_ModuleId",
                table: "UserModule",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
