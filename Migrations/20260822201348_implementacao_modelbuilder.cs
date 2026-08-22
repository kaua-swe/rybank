using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace src.Migrations
{
    /// <inheritdoc />
    public partial class implementacao_modelbuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallet_UsuarioId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Balance_UsuarioId",
                table: "Balance");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UsuarioId_Conta",
                table: "Wallet",
                columns: new[] { "UsuarioId", "Conta" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_Codigo",
                table: "Ticket",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pix_Chave",
                table: "Pix",
                column: "Chave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Balance_UsuarioId_Conta",
                table: "Balance",
                columns: new[] { "UsuarioId", "Conta" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_CPF",
                table: "Account",
                column: "CPF",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallet_UsuarioId_Conta",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_Codigo",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Pix_Chave",
                table: "Pix");

            migrationBuilder.DropIndex(
                name: "IX_Balance_UsuarioId_Conta",
                table: "Balance");

            migrationBuilder.DropIndex(
                name: "IX_Account_CPF",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UsuarioId",
                table: "Wallet",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_UsuarioId",
                table: "Balance",
                column: "UsuarioId");
        }
    }
}
