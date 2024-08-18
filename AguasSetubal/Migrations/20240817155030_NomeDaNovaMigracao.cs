using Microsoft.EntityFrameworkCore.Migrations;

namespace AguasSetubal.Migrations
{
    public partial class NomeDaNovaMigracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LeituraAtual",
                table: "Faturas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeituraAnterior",
                table: "Faturas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeituraAnterior",
                table: "Faturas");

            migrationBuilder.AlterColumn<int>(
                name: "LeituraAtual",
                table: "Faturas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
