using Microsoft.EntityFrameworkCore.Migrations;

namespace AguasSetubal.Migrations
{
    public partial class fatura4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "M3Gastos",
                table: "LeituraContadores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "M3Gastos",
                table: "LeituraContadores");
        }
    }
}
