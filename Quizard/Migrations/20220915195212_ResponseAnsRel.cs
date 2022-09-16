using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class ResponseAnsRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");
        }
    }
}
