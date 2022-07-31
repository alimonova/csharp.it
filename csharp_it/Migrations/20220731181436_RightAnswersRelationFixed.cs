using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace csharp_it.Migrations
{
    public partial class RightAnswersRelationFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Answers_RightAnswerId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_RightAnswerId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RightAnswerId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RightAnswerId1",
                table: "Questions");

            migrationBuilder.AddColumn<bool>(
                name: "RightAnswer",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RightAnswer",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "RightAnswerId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RightAnswerId1",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_RightAnswerId1",
                table: "Questions",
                column: "RightAnswerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Answers_RightAnswerId1",
                table: "Questions",
                column: "RightAnswerId1",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
