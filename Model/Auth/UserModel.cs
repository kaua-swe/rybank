using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Model.Account;
using src.Model.Bank;
using src.Model.Movement;
using src.Model.Pix;
using src.Model.Ticket;
using src.Model.Trasnfer;

namespace src.Model.Auth
{
    public class UserModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public AccountModel? Account { get; set; }
        
        public List<BalanceModel?> Balance { get; set; } = new();

        public List<WalletModel?> Wallet { get; set; } = new();

        public List<PixModel?> Pix { get; set; } = new();

        [InverseProperty("OrigemUser")]
        public List<TransferModel?> TransEnviadas { get; set; } = new();

        [InverseProperty("DestinoUser")]
        public List<TransferModel?> TransRecebidas { get; set; } = new();

        public List<MovementModel?> Movement { get; set; } = new();

    }
}