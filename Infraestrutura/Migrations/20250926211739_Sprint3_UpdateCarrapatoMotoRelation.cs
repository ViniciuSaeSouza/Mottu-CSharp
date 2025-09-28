using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Sprint3_UpdateCarrapatoMotoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOTOS_CARRAPATOS_IdCarrapato",
                table: "MOTOS");

            migrationBuilder.DropColumn(
                name: "IdMoto",
                table: "CARRAPATOS");

            migrationBuilder.AddForeignKey(
                name: "FK_MOTOS_CARRAPATOS_IdCarrapato",
                table: "MOTOS",
                column: "IdCarrapato",
                principalTable: "CARRAPATOS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MOTOS_CARRAPATOS_IdCarrapato",
                table: "MOTOS");

            migrationBuilder.AddColumn<int>(
                name: "IdMoto",
                table: "CARRAPATOS",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MOTOS_CARRAPATOS_IdCarrapato",
                table: "MOTOS",
                column: "IdCarrapato",
                principalTable: "CARRAPATOS",
                principalColumn: "Id");
        }
    }
}
