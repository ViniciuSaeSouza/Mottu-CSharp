using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Sprint3_renameTablesToUpper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motos_Patios_idPatio",
                table: "Motos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patios",
                table: "Patios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Motos",
                table: "Motos");

            migrationBuilder.RenameTable(
                name: "Patios",
                newName: "PATIO");

            migrationBuilder.RenameTable(
                name: "Motos",
                newName: "MOTO");

            migrationBuilder.RenameIndex(
                name: "IX_Patios_Nome",
                table: "PATIO",
                newName: "IX_PATIO_Nome");

            migrationBuilder.RenameIndex(
                name: "IX_Motos_Placa",
                table: "MOTO",
                newName: "IX_MOTO_Placa");

            migrationBuilder.RenameIndex(
                name: "IX_Motos_idPatio",
                table: "MOTO",
                newName: "IX_MOTO_idPatio");

            migrationBuilder.RenameIndex(
                name: "IX_Motos_Chassi",
                table: "MOTO",
                newName: "IX_MOTO_Chassi");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PATIO",
                table: "PATIO",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MOTO",
                table: "MOTO",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MOTO_PATIO_idPatio",
                table: "MOTO",
                column: "idPatio",
                principalTable: "PATIO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOTO_PATIO_idPatio",
                table: "MOTO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PATIO",
                table: "PATIO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MOTO",
                table: "MOTO");

            migrationBuilder.RenameTable(
                name: "PATIO",
                newName: "Patios");

            migrationBuilder.RenameTable(
                name: "MOTO",
                newName: "Motos");

            migrationBuilder.RenameIndex(
                name: "IX_PATIO_Nome",
                table: "Patios",
                newName: "IX_Patios_Nome");

            migrationBuilder.RenameIndex(
                name: "IX_MOTO_Placa",
                table: "Motos",
                newName: "IX_Motos_Placa");

            migrationBuilder.RenameIndex(
                name: "IX_MOTO_idPatio",
                table: "Motos",
                newName: "IX_Motos_idPatio");

            migrationBuilder.RenameIndex(
                name: "IX_MOTO_Chassi",
                table: "Motos",
                newName: "IX_Motos_Chassi");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patios",
                table: "Patios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Motos",
                table: "Motos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Motos_Patios_idPatio",
                table: "Motos",
                column: "idPatio",
                principalTable: "Patios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
