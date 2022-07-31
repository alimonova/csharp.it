using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace csharp_it.Migrations
{
    public partial class AccessTarifsRelationsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access",
                table: "Tarifs");

            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TarifAccess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TarifId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarifAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TarifAccess_Access_AccessId",
                        column: x => x.AccessId,
                        principalTable: "Access",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TarifAccess_Tarifs_TarifId",
                        column: x => x.TarifId,
                        principalTable: "Tarifs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TarifAccess_AccessId",
                table: "TarifAccess",
                column: "AccessId");

            migrationBuilder.CreateIndex(
                name: "IX_TarifAccess_TarifId",
                table: "TarifAccess",
                column: "TarifId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TarifAccess");

            migrationBuilder.DropTable(
                name: "Access");

            migrationBuilder.AddColumn<string>(
                name: "Access",
                table: "Tarifs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
