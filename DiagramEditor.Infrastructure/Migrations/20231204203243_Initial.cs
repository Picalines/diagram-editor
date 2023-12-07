using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiagramEditor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Users",
                    columns: table =>
                        new
                        {
                            Id = table.Column<Guid>(
                                type: "char(36)",
                                nullable: false,
                                collation: "ascii_general_ci"
                            ),
                            Login = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            PasswordHash = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            DisplayName = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            AvatarUrl = table
                                .Column<string>(type: "longtext", nullable: true)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            CreatedDate = table.Column<DateTime>(
                                type: "datetime(6)",
                                nullable: false
                            ),
                            IsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false)
                        },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Users", x => x.Id);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Diagrams",
                    columns: table =>
                        new
                        {
                            Id = table.Column<Guid>(
                                type: "char(36)",
                                nullable: false,
                                collation: "ascii_general_ci"
                            ),
                            CreatorId = table.Column<Guid>(
                                type: "char(36)",
                                nullable: false,
                                collation: "ascii_general_ci"
                            ),
                            Name = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            CreatedDate = table.Column<DateTime>(
                                type: "datetime(6)",
                                nullable: false
                            ),
                            UpdatedDate = table.Column<DateTime>(
                                type: "datetime(6)",
                                nullable: false
                            ),
                            ViewsCount = table.Column<int>(type: "int", nullable: false)
                        },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Diagrams", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Diagrams_Users_CreatorId",
                            column: x => x.CreatorId,
                            principalTable: "Users",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "DiagramElement",
                    columns: table =>
                        new
                        {
                            Id = table.Column<Guid>(
                                type: "char(36)",
                                nullable: false,
                                collation: "ascii_general_ci"
                            ),
                            DiagramId = table.Column<Guid>(
                                type: "char(36)",
                                nullable: false,
                                collation: "ascii_general_ci"
                            ),
                            Type = table.Column<int>(type: "int", nullable: false),
                            OriginX = table.Column<float>(type: "float", nullable: false),
                            OriginY = table.Column<float>(type: "float", nullable: false)
                        },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_DiagramElement", x => x.Id);
                        table.ForeignKey(
                            name: "FK_DiagramElement_Diagrams_DiagramId",
                            column: x => x.DiagramId,
                            principalTable: "Diagrams",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "DiagramEnvironment",
                    columns: table =>
                        new
                        {
                            Id = table.Column<Guid>(
                                type: "char(36)",
                                nullable: false,
                                collation: "ascii_general_ci"
                            ),
                            DiagramId = table.Column<Guid>(
                                type: "char(36)",
                                nullable: false,
                                collation: "ascii_general_ci"
                            ),
                            PublicUrl = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            PublicName = table
                                .Column<string>(type: "longtext", nullable: false)
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
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "DiagramElementProperty",
                    columns: table =>
                        new
                        {
                            Id = table.Column<Guid>(
                                type: "char(36)",
                                nullable: false,
                                collation: "ascii_general_ci"
                            ),
                            ElementId = table.Column<Guid>(
                                type: "char(36)",
                                nullable: false,
                                collation: "ascii_general_ci"
                            ),
                            Key = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            Value = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4")
                        },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_DiagramElementProperty", x => x.Id);
                        table.ForeignKey(
                            name: "FK_DiagramElementProperty_DiagramElement_ElementId",
                            column: x => x.ElementId,
                            principalTable: "DiagramElement",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DiagramElement_DiagramId",
                table: "DiagramElement",
                column: "DiagramId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_DiagramElementProperty_ElementId",
                table: "DiagramElementProperty",
                column: "ElementId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_DiagramEnvironment_DiagramId",
                table: "DiagramEnvironment",
                column: "DiagramId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Diagrams_CreatorId",
                table: "Diagrams",
                column: "CreatorId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "DiagramElementProperty");

            migrationBuilder.DropTable(name: "DiagramEnvironment");

            migrationBuilder.DropTable(name: "DiagramElement");

            migrationBuilder.DropTable(name: "Diagrams");

            migrationBuilder.DropTable(name: "Users");
        }
    }
}
