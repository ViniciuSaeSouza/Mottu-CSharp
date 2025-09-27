using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Sprint3_RefactorIdPatioFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOTOS_PATIOS_idPatio",
                table: "MOTOS");

            migrationBuilder.RenameColumn(
                name: "idPatio",
                table: "MOTOS",
                newName: "IdPatio");

            migrationBuilder.RenameIndex(
                name: "IX_MOTOS_idPatio",
                table: "MOTOS",
                newName: "IX_MOTOS_IdPatio");

            migrationBuilder.AddForeignKey(
                name: "FK_MOTOS_PATIOS_IdPatio",
                table: "MOTOS",
                column: "IdPatio",
                principalTable: "PATIOS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOTOS_PATIOS_IdPatio",
                table: "MOTOS");

            migrationBuilder.RenameColumn(
                name: "IdPatio",
                table: "MOTOS",
                newName: "idPatio");

            migrationBuilder.RenameIndex(
                name: "IX_MOTOS_IdPatio",
                table: "MOTOS",
                newName: "IX_MOTOS_idPatio");

            migrationBuilder.AddForeignKey(
                name: "FK_MOTOS_PATIOS_idPatio",
                table: "MOTOS",
                column: "idPatio",
                principalTable: "PATIOS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
