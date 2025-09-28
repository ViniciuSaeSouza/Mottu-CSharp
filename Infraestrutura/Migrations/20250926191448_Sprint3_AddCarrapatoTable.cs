using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Sprint3_AddCarrapatoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCarrapato",
                table: "MOTOS",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CARRAPATOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CodigoSerial = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: false),
                    StatusBateria = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    StatusDeUso = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdPatio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdMoto = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARRAPATOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CARRAPATOS_PATIOS_IdPatio",
                        column: x => x.IdPatio,
                        principalTable: "PATIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOTOS_IdCarrapato",
                table: "MOTOS",
                column: "IdCarrapato",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CARRAPATOS_CodigoSerial",
                table: "CARRAPATOS",
                column: "CodigoSerial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CARRAPATOS_IdPatio",
                table: "CARRAPATOS",
                column: "IdPatio");

            migrationBuilder.AddForeignKey(
                name: "FK_MOTOS_CARRAPATOS_IdCarrapato",
                table: "MOTOS",
                column: "IdCarrapato",
                principalTable: "CARRAPATOS",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOTOS_CARRAPATOS_IdCarrapato",
                table: "MOTOS");

            migrationBuilder.DropTable(
                name: "CARRAPATOS");

            migrationBuilder.DropIndex(
                name: "IX_MOTOS_IdCarrapato",
                table: "MOTOS");

            migrationBuilder.DropColumn(
                name: "IdCarrapato",
                table: "MOTOS");
        }
    }
}
