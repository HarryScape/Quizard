using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class ParentID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_ParentQuestionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ParentQuestionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ParentQuestionId",
                table: "Questions");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ParentId",
                table: "Questions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_ParentId",
                table: "Questions",
                column: "ParentId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_ParentId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ParentId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "ParentQuestionId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ParentQuestionId",
                table: "Questions",
                column: "ParentQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_ParentQuestionId",
                table: "Questions",
                column: "ParentQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
