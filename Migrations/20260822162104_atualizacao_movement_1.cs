using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace src.Migrations
{
    /// <inheritdoc />
    public partial class atualizacao_movement_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movement_User_UserModelId",
                table: "Movement");

            migrationBuilder.DropIndex(
                name: "IX_Movement_UserModelId",
                table: "Movement");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Movement");

            migrationBuilder.AddColumn<decimal>(
                name: "Valor",
                table: "Ticket",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Movement_UsuarioId",
                table: "Movement",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movement_User_UsuarioId",
                table: "Movement",
                column: "UsuarioId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movement_User_UsuarioId",
                table: "Movement");

            migrationBuilder.DropIndex(
                name: "IX_Movement_UsuarioId",
                table: "Movement");

            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Ticket");

            migrationBuilder.AddColumn<Guid>(
                name: "UserModelId",
                table: "Movement",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movement_UserModelId",
                table: "Movement",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movement_User_UserModelId",
                table: "Movement",
                column: "UserModelId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
