using Microsoft.EntityFrameworkCore.Migrations;

namespace AguasSetubal.Migrations
{
    public partial class AddNumeroContadorColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Faturas_LeituraContadorId",
                table: "Faturas");

            migrationBuilder.AddColumn<string>(
                name: "NumeroContador",
                table: "LeituraContadores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_LeituraContadorId",
                table: "Faturas",
                column: "LeituraContadorId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Faturas_LeituraContadorId",
                table: "Faturas");

            migrationBuilder.DropColumn(
                name: "NumeroContador",
                table: "LeituraContadores");

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_LeituraContadorId",
                table: "Faturas",
                column: "LeituraContadorId");
        }
    }
}
