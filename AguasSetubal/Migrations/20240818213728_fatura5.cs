using Microsoft.EntityFrameworkCore.Migrations;

namespace AguasSetubal.Migrations
{
    public partial class fatura5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "M3Gastos",
                table: "Faturas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "M3Gastos",
                table: "Faturas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
