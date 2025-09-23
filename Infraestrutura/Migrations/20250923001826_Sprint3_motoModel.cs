using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Sprint3_motoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motos_Filiais_IdFilial",
                table: "Motos");

            migrationBuilder.DropTable(
                name: "Filiais");

            migrationBuilder.RenameColumn(
                name: "IdFilial",
                table: "Motos",
                newName: "idPatio");

            migrationBuilder.RenameIndex(
                name: "IX_Motos_IdFilial",
                table: "Motos",
                newName: "IX_Motos_idPatio");

            migrationBuilder.AddColumn<int>(
                name: "Zona",
                table: "Motos",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Patios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patios_Nome",
                table: "Patios",
                column: "Nome",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Motos_Patios_idPatio",
                table: "Motos",
                column: "idPatio",
                principalTable: "Patios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motos_Patios_idPatio",
                table: "Motos");

            migrationBuilder.DropTable(
                name: "Patios");

            migrationBuilder.DropColumn(
                name: "Zona",
                table: "Motos");

            migrationBuilder.RenameColumn(
                name: "idPatio",
                table: "Motos",
                newName: "IdFilial");

            migrationBuilder.RenameIndex(
                name: "IX_Motos_idPatio",
                table: "Motos",
                newName: "IX_Motos_IdFilial");

            migrationBuilder.CreateTable(
                name: "Filiais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Endereco = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiais", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Filiais_Nome",
                table: "Filiais",
                column: "Nome",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Motos_Filiais_IdFilial",
                table: "Motos",
                column: "IdFilial",
                principalTable: "Filiais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
