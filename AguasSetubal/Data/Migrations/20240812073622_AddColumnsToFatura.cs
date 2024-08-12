using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AguasSetubal.Data.Migrations
{
    public partial class AddColumnsToFatura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeiturasContadores_Clientes_ClienteId",
                table: "LeiturasContadores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeiturasContadores",
                table: "LeiturasContadores");

            migrationBuilder.RenameTable(
                name: "LeiturasContadores",
                newName: "LeituraContador");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Faturas",
                newName: "ValorTotal");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Faturas",
                newName: "DataEmissao");

            migrationBuilder.RenameIndex(
                name: "IX_LeiturasContadores_ClienteId",
                table: "LeituraContador",
                newName: "IX_LeituraContador_ClienteId");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Faturas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeituraContadorId",
                table: "Faturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataLeituraAnterior",
                table: "LeituraContador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LeituraAnterior",
                table: "LeituraContador",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorPagar",
                table: "LeituraContador",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeituraContador",
                table: "LeituraContador",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_LeituraContadorId",
                table: "Faturas",
                column: "LeituraContadorId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Faturas_LeituraContador_LeituraContadorId",
            //    table: "Faturas",
            //    column: "LeituraContadorId",
            //    principalTable: "LeituraContador",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeituraContador_Clientes_ClienteId",
                table: "LeituraContador",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Faturas_LeituraContador_LeituraContadorId",
            //    table: "Faturas");

            migrationBuilder.DropForeignKey(
                name: "FK_LeituraContador_Clientes_ClienteId",
                table: "LeituraContador");

            migrationBuilder.DropIndex(
                name: "IX_Faturas_LeituraContadorId",
                table: "Faturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeituraContador",
                table: "LeituraContador");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Faturas");

            migrationBuilder.DropColumn(
                name: "LeituraContadorId",
                table: "Faturas");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "DataLeituraAnterior",
                table: "LeituraContador");

            migrationBuilder.DropColumn(
                name: "LeituraAnterior",
                table: "LeituraContador");

            migrationBuilder.DropColumn(
                name: "ValorPagar",
                table: "LeituraContador");

            migrationBuilder.RenameTable(
                name: "LeituraContador",
                newName: "LeiturasContadores");

            migrationBuilder.RenameColumn(
                name: "ValorTotal",
                table: "Faturas",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "DataEmissao",
                table: "Faturas",
                newName: "Data");

            migrationBuilder.RenameIndex(
                name: "IX_LeituraContador_ClienteId",
                table: "LeiturasContadores",
                newName: "IX_LeiturasContadores_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeiturasContadores",
                table: "LeiturasContadores",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeiturasContadores_Clientes_ClienteId",
                table: "LeiturasContadores",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
