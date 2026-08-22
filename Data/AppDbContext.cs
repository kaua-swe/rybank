using Microsoft.EntityFrameworkCore;
using src.Model.Account;
using src.Model.Auth;
using src.Model.Bank;
using src.Model.Movement;
using src.Model.Pix;
using src.Model.Ticket;
using src.Model.Trasnfer;

namespace src.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        public DbSet<UserModel> User { get; set; }
        public DbSet<AccountModel> Account { get; set; }
        public DbSet<BalanceModel> Balance { get; set; }        
        public DbSet<WalletModel> Wallet { get; set; }
        public DbSet<PixModel> Pix { get; set; }
        public DbSet<TransferModel> Transfer { get; set; }
        public DbSet<MovementModel> Movement { get; set; }
        public DbSet<TicketModel> Ticket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<AccountModel>().HasIndex(c => c.CPF).IsUnique();
            modelBuilder.Entity<BalanceModel>().HasIndex(u => new {u.UsuarioId, u.Conta} ).IsUnique();
            modelBuilder.Entity<WalletModel>().HasIndex(u => new {u.UsuarioId, u.Conta} ).IsUnique();
            modelBuilder.Entity<PixModel>().HasIndex(p => p.Chave).IsUnique();
            modelBuilder.Entity<TicketModel>().HasIndex(t => t.Codigo).IsUnique();

        }

    }
}