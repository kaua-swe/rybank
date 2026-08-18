using Microsoft.EntityFrameworkCore;
using rybank.estudo.Models;
using rybank.Models;

namespace rybank.estudo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        public DbSet<UserModel> Users { get; set; }

        public DbSet<SaldoModel> Saldo { get; set; }

        public DbSet<DepositModel> Deposito { get; set; }

        public DbSet<TransferenciaModel> Transferencias { get; set; }

        public DbSet<MovimentacaoModel> Movimentacao { get; set; }

        public DbSet<BoletoModel> Boleto { get; set; }

        public DbSet<PixModel> Pix { get; set; }
    }
}