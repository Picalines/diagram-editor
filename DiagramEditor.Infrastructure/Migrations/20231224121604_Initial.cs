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
            migrationBuilder.DropForeignKey(
                name: "FK_Diagrams_Users_CreatorId",
                table: "Diagrams");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Diagrams",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Diagrams_CreatorId",
                table: "Diagrams",
                newName: "IX_Diagrams_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagrams_Users_UserId",
                table: "Diagrams",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagrams_Users_UserId",
                table: "Diagrams");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Diagrams",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Diagrams_UserId",
                table: "Diagrams",
                newName: "IX_Diagrams_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagrams_Users_CreatorId",
                table: "Diagrams",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
