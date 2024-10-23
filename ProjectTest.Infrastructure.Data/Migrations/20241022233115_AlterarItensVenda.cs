using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTest.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterarItensVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "cancelada",
                table: "venda",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "cancelada",
                table: "item_venda",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cancelada",
                table: "item_venda");

            migrationBuilder.AlterColumn<bool>(
                name: "cancelada",
                table: "venda",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
