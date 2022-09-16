using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class ResponseDeleteConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_UserQuizAttempts_UserQuizAttemptId",
                table: "UserQuestionResponses");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "UserQuestionResponses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_UserQuizAttempts_UserQuizAttemptId",
                table: "UserQuestionResponses",
                column: "UserQuizAttemptId",
                principalTable: "UserQuizAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_UserQuizAttempts_UserQuizAttemptId",
                table: "UserQuestionResponses");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "UserQuestionResponses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_Answers_AnswerId",
                table: "UserQuestionResponses",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_UserQuizAttempts_UserQuizAttemptId",
                table: "UserQuestionResponses",
                column: "UserQuizAttemptId",
                principalTable: "UserQuizAttempts",
                principalColumn: "Id");
        }
    }
}
