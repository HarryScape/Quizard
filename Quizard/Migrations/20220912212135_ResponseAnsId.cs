using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class ResponseAnsId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "UserQuestionResponses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionResponses_AnswerId",
                table: "UserQuestionResponses",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses");

            migrationBuilder.DropIndex(
                name: "IX_UserQuestionResponses_AnswerId",
                table: "UserQuestionResponses");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "UserQuestionResponses");
        }
    }
}
