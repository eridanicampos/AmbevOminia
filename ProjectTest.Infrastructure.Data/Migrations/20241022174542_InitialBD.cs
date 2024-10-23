using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTest.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    senha = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venda",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    numero_venda = table.Column<long>(type: "bigint", nullable: false),
                    data_venda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome_cliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    filial = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cancelada = table.Column<bool>(type: "bit", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venda", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "acesso_usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acesso_usuario", x => x.id);
                    table.ForeignKey(
                        name: "FK_acesso_usuario_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_venda",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    produto_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome_produto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    valor_unitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    venda_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: false, comment: "The id of the user who did create"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this entity was created in this DB"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was modified the last time"),
                    update_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the last modification"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "The field that identifies that the entity was deleted"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this entity was deleted in this DB"),
                    deleted_by_user_id = table.Column<string>(type: "VARCHAR(100)", nullable: true, comment: "The id of the user who did the delete")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_venda", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_venda_venda_venda_id",
                        column: x => x.venda_id,
                        principalTable: "venda",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_acesso_usuario_usuario_id",
                table: "acesso_usuario",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_venda_id",
                table: "item_venda",
                column: "venda_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "acesso_usuario");

            migrationBuilder.DropTable(
                name: "item_venda");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "venda");
        }
    }
}
