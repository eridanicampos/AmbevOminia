using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTest.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterarItemVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "desconto",
                table: "item_venda");

            migrationBuilder.AddColumn<decimal>(
                name: "desconto_valor_unitario",
                table: "item_venda",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "desconto_valor_unitario",
                table: "item_venda");

            migrationBuilder.AddColumn<decimal>(
                name: "desconto",
                table: "item_venda",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
