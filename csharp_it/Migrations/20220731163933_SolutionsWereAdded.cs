using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace csharp_it.Migrations
{
    public partial class SolutionsWereAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TarifAccess_Access_AccessId",
                table: "TarifAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_TarifAccess_Tarifs_TarifId",
                table: "TarifAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TarifAccess",
                table: "TarifAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Access",
                table: "Access");

            migrationBuilder.RenameTable(
                name: "TarifAccess",
                newName: "TarifAccesses");

            migrationBuilder.RenameTable(
                name: "Access",
                newName: "Accesses");

            migrationBuilder.RenameIndex(
                name: "IX_TarifAccess_TarifId",
                table: "TarifAccesses",
                newName: "IX_TarifAccesses_TarifId");

            migrationBuilder.RenameIndex(
                name: "IX_TarifAccess_AccessId",
                table: "TarifAccesses",
                newName: "IX_TarifAccesses_AccessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TarifAccesses",
                table: "TarifAccesses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accesses",
                table: "Accesses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    Mark = table.Column<double>(type: "float", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solutions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Solutions_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_TaskId",
                table: "Solutions",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_UserId",
                table: "Solutions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TarifAccesses_Accesses_AccessId",
                table: "TarifAccesses",
                column: "AccessId",
                principalTable: "Accesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TarifAccesses_Tarifs_TarifId",
                table: "TarifAccesses",
                column: "TarifId",
                principalTable: "Tarifs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TarifAccesses_Accesses_AccessId",
                table: "TarifAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_TarifAccesses_Tarifs_TarifId",
                table: "TarifAccesses");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TarifAccesses",
                table: "TarifAccesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accesses",
                table: "Accesses");

            migrationBuilder.RenameTable(
                name: "TarifAccesses",
                newName: "TarifAccess");

            migrationBuilder.RenameTable(
                name: "Accesses",
                newName: "Access");

            migrationBuilder.RenameIndex(
                name: "IX_TarifAccesses_TarifId",
                table: "TarifAccess",
                newName: "IX_TarifAccess_TarifId");

            migrationBuilder.RenameIndex(
                name: "IX_TarifAccesses_AccessId",
                table: "TarifAccess",
                newName: "IX_TarifAccess_AccessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TarifAccess",
                table: "TarifAccess",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Access",
                table: "Access",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TarifAccess_Access_AccessId",
                table: "TarifAccess",
                column: "AccessId",
                principalTable: "Access",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TarifAccess_Tarifs_TarifId",
                table: "TarifAccess",
                column: "TarifId",
                principalTable: "Tarifs",
                principalColumn: "Id");
        }
    }
}
