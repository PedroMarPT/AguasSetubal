using Microsoft.EntityFrameworkCore.Migrations;

namespace AguasSetubal.Data.Migrations
{
    public partial class Faturas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Faturas_LeituraContador_LeituraContadorId",
            //    table: "Faturas");

            migrationBuilder.AlterColumn<int>(
                name: "LeituraContadorId",
                table: "Faturas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Faturas_LeituraContador_LeituraContadorId",
            //    table: "Faturas",
            //    column: "LeituraContadorId",
            //    principalTable: "LeituraContador",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Faturas_LeituraContador_LeituraContadorId",
            //    table: "Faturas");

            migrationBuilder.AlterColumn<int>(
                name: "LeituraContadorId",
                table: "Faturas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Faturas_LeituraContador_LeituraContadorId",
            //    table: "Faturas",
            //    column: "LeituraContadorId",
            //    principalTable: "LeituraContador",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
