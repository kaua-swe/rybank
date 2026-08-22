using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace src.Migrations
{
    /// <inheritdoc />
    public partial class implementacao_transfer_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_User_UsuarioId",
                table: "Transfer");

            migrationBuilder.DropIndex(
                name: "IX_Transfer_UsuarioId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Transfer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Transfer",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_UsuarioId",
                table: "Transfer",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_User_UsuarioId",
                table: "Transfer",
                column: "UsuarioId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
