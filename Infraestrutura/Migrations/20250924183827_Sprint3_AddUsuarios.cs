using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Sprint3_AddUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOTOS_PATIO_idPatio",
                table: "MOTOS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PATIO",
                table: "PATIO");

            migrationBuilder.RenameTable(
                name: "PATIO",
                newName: "PATIOS");

            migrationBuilder.RenameIndex(
                name: "IX_PATIO_Nome",
                table: "PATIOS",
                newName: "IX_PATIOS_Nome");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PATIOS",
                table: "PATIOS",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    IdPatio = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_USUARIOS_PATIOS_IdPatio",
                        column: x => x.IdPatio,
                        principalTable: "PATIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_Email",
                table: "USUARIOS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_IdPatio",
                table: "USUARIOS",
                column: "IdPatio");

            migrationBuilder.AddForeignKey(
                name: "FK_MOTOS_PATIOS_idPatio",
                table: "MOTOS",
                column: "idPatio",
                principalTable: "PATIOS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOTOS_PATIOS_idPatio",
                table: "MOTOS");

            migrationBuilder.DropTable(
                name: "USUARIOS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PATIOS",
                table: "PATIOS");

            migrationBuilder.RenameTable(
                name: "PATIOS",
                newName: "PATIO");

            migrationBuilder.RenameIndex(
                name: "IX_PATIOS_Nome",
                table: "PATIO",
                newName: "IX_PATIO_Nome");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PATIO",
                table: "PATIO",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MOTOS_PATIO_idPatio",
                table: "MOTOS",
                column: "idPatio",
                principalTable: "PATIO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
