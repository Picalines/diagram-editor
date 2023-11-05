using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiagramEditor.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagram_DiagramEnvironment_Id",
                table: "Diagram");

            migrationBuilder.AddColumn<int>(
                name: "DiagramId",
                table: "DiagramEnvironment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Diagram",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_DiagramEnvironment_DiagramId",
                table: "DiagramEnvironment",
                column: "DiagramId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiagramEnvironment_Diagram_DiagramId",
                table: "DiagramEnvironment",
                column: "DiagramId",
                principalTable: "Diagram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiagramEnvironment_Diagram_DiagramId",
                table: "DiagramEnvironment");

            migrationBuilder.DropIndex(
                name: "IX_DiagramEnvironment_DiagramId",
                table: "DiagramEnvironment");

            migrationBuilder.DropColumn(
                name: "DiagramId",
                table: "DiagramEnvironment");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Diagram",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Diagram_DiagramEnvironment_Id",
                table: "Diagram",
                column: "Id",
                principalTable: "DiagramEnvironment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
