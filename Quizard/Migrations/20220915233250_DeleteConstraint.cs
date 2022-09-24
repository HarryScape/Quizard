using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class DeleteConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_UserQuizAttempts_UserQuizAttemptId",
                table: "UserQuestionResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_UserQuizAttempts_UserQuizAttemptId",
                table: "UserQuestionResponses",
                column: "UserQuizAttemptId",
                principalTable: "UserQuizAttempts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_UserQuizAttempts_UserQuizAttemptId",
                table: "UserQuestionResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_UserQuizAttempts_UserQuizAttemptId",
                table: "UserQuestionResponses",
                column: "UserQuizAttemptId",
                principalTable: "UserQuizAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
