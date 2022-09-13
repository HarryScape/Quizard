using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class TakeQuizTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserQuizAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: true),
                    TimeStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsMarked = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseFeedback = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuizAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuizAttempts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserQuizAttempts_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserQuestionResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UserQuizAttemptId = table.Column<int>(type: "int", nullable: false),
                    AnswerResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarkAwarded = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestionResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestionResponses_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserQuestionResponses_UserQuizAttempts_UserQuizAttemptId",
                        column: x => x.UserQuizAttemptId,
                        principalTable: "UserQuizAttempts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionResponses_QuestionId",
                table: "UserQuestionResponses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionResponses_UserQuizAttemptId",
                table: "UserQuestionResponses",
                column: "UserQuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAttempts_QuizId",
                table: "UserQuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAttempts_UserId",
                table: "UserQuizAttempts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserQuestionResponses");

            migrationBuilder.DropTable(
                name: "UserQuizAttempts");
        }
    }
}
