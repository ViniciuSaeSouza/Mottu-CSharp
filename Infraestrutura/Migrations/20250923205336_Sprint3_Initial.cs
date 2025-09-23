using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Sprint3_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOTOS_MOTTU",
                columns: table => new
                {
                    ID_MOTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PLACA = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: false),
                    CHASSI = table.Column<string>(type: "NVARCHAR2(17)", maxLength: 17, nullable: false),
                    ID_MODELO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTOS_MOTTU", x => x.ID_MOTO);
                });

            migrationBuilder.CreateTable(
                name: "PATIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MOTOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Placa = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: false),
                    Chassi = table.Column<string>(type: "NVARCHAR2(17)", maxLength: 17, nullable: false),
                    Modelo = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Zona = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    idPatio = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MOTOS_PATIO_idPatio",
                        column: x => x.idPatio,
                        principalTable: "PATIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOTOS_Chassi",
                table: "MOTOS",
                column: "Chassi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MOTOS_idPatio",
                table: "MOTOS",
                column: "idPatio");

            migrationBuilder.CreateIndex(
                name: "IX_MOTOS_Placa",
                table: "MOTOS",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PATIO_Nome",
                table: "PATIO",
                column: "Nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOTOS");

            migrationBuilder.DropTable(
                name: "MOTOS_MOTTU");

            migrationBuilder.DropTable(
                name: "PATIO");
        }
    }
}
