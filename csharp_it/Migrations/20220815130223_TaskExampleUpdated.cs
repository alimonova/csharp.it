using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace csharp_it.Migrations
{
    public partial class TaskExampleUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Example",
                table: "Tasks",
                newName: "ExampleOutput");

            migrationBuilder.AddColumn<string>(
                name: "ExampleInput",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExampleInput",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ExampleOutput",
                table: "Tasks",
                newName: "Example");
        }
    }
}
