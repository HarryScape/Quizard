using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class ModuleEnroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_AspNetUsers_UserId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_UserId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Modules");

            migrationBuilder.AddColumn<string>(
                name: "ModuleLeaderId",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ModuleUser",
                columns: table => new
                {
                    ModuleUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserModulesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleUser", x => new { x.ModuleUsersId, x.UserModulesId });
                    table.ForeignKey(
                        name: "FK_ModuleUser_AspNetUsers_ModuleUsersId",
                        column: x => x.ModuleUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleUser_Modules_UserModulesId",
                        column: x => x.UserModulesId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleUser_UserModulesId",
                table: "ModuleUser",
                column: "UserModulesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleUser");

            migrationBuilder.DropColumn(
                name: "ModuleLeaderId",
                table: "Modules");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Modules",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_UserId",
                table: "Modules",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_AspNetUsers_UserId",
                table: "Modules",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
