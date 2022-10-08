using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizard.Migrations
{
    public partial class ExtraTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExtraTime",
                table: "Quizzes",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraTime",
                table: "Quizzes");
        }
    }
}
