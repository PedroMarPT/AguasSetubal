using Microsoft.EntityFrameworkCore.Migrations;

namespace AguasSetubal.Migrations
{
    public partial class updatedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faturas_LeituraContadores_LeituraContadorId",
                table: "Faturas");

            migrationBuilder.DropIndex(
                name: "IX_Faturas_LeituraContadorId",
                table: "Faturas");

            migrationBuilder.DropColumn(
                name: "LeituraContadorId",
                table: "Faturas");

            migrationBuilder.AddColumn<int>(
                name: "FaturaId",
                table: "LeituraContadores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LeituraAnterior",
                table: "Faturas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LeituraAtual",
                table: "Faturas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_LeituraContadores_FaturaId",
                table: "LeituraContadores",
                column: "FaturaId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeituraContadores_Faturas_FaturaId",
                table: "LeituraContadores",
                column: "FaturaId",
                principalTable: "Faturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeituraContadores_Faturas_FaturaId",
                table: "LeituraContadores");

            migrationBuilder.DropIndex(
                name: "IX_LeituraContadores_FaturaId",
                table: "LeituraContadores");

            migrationBuilder.DropColumn(
                name: "FaturaId",
                table: "LeituraContadores");

            migrationBuilder.DropColumn(
                name: "LeituraAnterior",
                table: "Faturas");

            migrationBuilder.DropColumn(
                name: "LeituraAtual",
                table: "Faturas");

            migrationBuilder.AddColumn<int>(
                name: "LeituraContadorId",
                table: "Faturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_LeituraContadorId",
                table: "Faturas",
                column: "LeituraContadorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Faturas_LeituraContadores_LeituraContadorId",
                table: "Faturas",
                column: "LeituraContadorId",
                principalTable: "LeituraContadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
