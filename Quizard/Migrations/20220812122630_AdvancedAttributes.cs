using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class AdvancedAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequiredQuestions",
                table: "Sections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deployed",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Shuffled",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "Quizzes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ErrorMargin",
                table: "Questions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mark",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NegativeMark",
                table: "Questions",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredQuestions",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "Deployed",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Shuffled",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "ErrorMargin",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "NegativeMark",
                table: "Questions");
        }
    }
}
