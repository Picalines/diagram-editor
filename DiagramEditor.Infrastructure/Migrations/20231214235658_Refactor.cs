using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiagramEditor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiagramElement_Diagrams_DiagramId",
                table: "DiagramElement");

            migrationBuilder.DropForeignKey(
                name: "FK_DiagramElementProperty_DiagramElement_ElementId",
                table: "DiagramElementProperty");

            migrationBuilder.DropTable(
                name: "DiagramEnvironment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiagramElementProperty",
                table: "DiagramElementProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiagramElement",
                table: "DiagramElement");

            migrationBuilder.DropColumn(
                name: "ViewsCount",
                table: "Diagrams");

            migrationBuilder.RenameTable(
                name: "DiagramElementProperty",
                newName: "DiagramElementProperties");

            migrationBuilder.RenameTable(
                name: "DiagramElement",
                newName: "DiagramElements");

            migrationBuilder.RenameIndex(
                name: "IX_DiagramElementProperty_ElementId",
                table: "DiagramElementProperties",
                newName: "IX_DiagramElementProperties_ElementId");

            migrationBuilder.RenameIndex(
                name: "IX_DiagramElement_DiagramId",
                table: "DiagramElements",
                newName: "IX_DiagramElements_DiagramId");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Diagrams",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiagramElementProperties",
                table: "DiagramElementProperties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiagramElements",
                table: "DiagramElements",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DiagramEnvironments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DiagramId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ViewsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagramEnvironments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagramEnvironments_Diagrams_DiagramId",
                        column: x => x.DiagramId,
                        principalTable: "Diagrams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DiagramEnvironments_DiagramId",
                table: "DiagramEnvironments",
                column: "DiagramId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiagramElementProperties_DiagramElements_ElementId",
                table: "DiagramElementProperties",
                column: "ElementId",
                principalTable: "DiagramElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiagramElements_Diagrams_DiagramId",
                table: "DiagramElements",
                column: "DiagramId",
                principalTable: "Diagrams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiagramElementProperties_DiagramElements_ElementId",
                table: "DiagramElementProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_DiagramElements_Diagrams_DiagramId",
                table: "DiagramElements");

            migrationBuilder.DropTable(
                name: "DiagramEnvironments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiagramElements",
                table: "DiagramElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiagramElementProperties",
                table: "DiagramElementProperties");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Diagrams");

            migrationBuilder.RenameTable(
                name: "DiagramElements",
                newName: "DiagramElement");

            migrationBuilder.RenameTable(
                name: "DiagramElementProperties",
                newName: "DiagramElementProperty");

            migrationBuilder.RenameIndex(
                name: "IX_DiagramElements_DiagramId",
                table: "DiagramElement",
                newName: "IX_DiagramElement_DiagramId");

            migrationBuilder.RenameIndex(
                name: "IX_DiagramElementProperties_ElementId",
                table: "DiagramElementProperty",
                newName: "IX_DiagramElementProperty_ElementId");

            migrationBuilder.AddColumn<int>(
                name: "ViewsCount",
                table: "Diagrams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiagramElement",
                table: "DiagramElement",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiagramElementProperty",
                table: "DiagramElementProperty",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DiagramEnvironment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DiagramId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PublicName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublicUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagramEnvironment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagramEnvironment_Diagrams_DiagramId",
                        column: x => x.DiagramId,
                        principalTable: "Diagrams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DiagramEnvironment_DiagramId",
                table: "DiagramEnvironment",
                column: "DiagramId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiagramElement_Diagrams_DiagramId",
                table: "DiagramElement",
                column: "DiagramId",
                principalTable: "Diagrams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiagramElementProperty_DiagramElement_ElementId",
                table: "DiagramElementProperty",
                column: "ElementId",
                principalTable: "DiagramElement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
