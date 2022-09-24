using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class NullAtempt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_Questions_QuestionId",
                table: "UserQuestionResponses");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "UserQuizAttempts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_Questions_QuestionId",
                table: "UserQuestionResponses",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionResponses_Questions_QuestionId",
                table: "UserQuestionResponses");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "UserQuizAttempts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionResponses_Questions_QuestionId",
                table: "UserQuestionResponses",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
