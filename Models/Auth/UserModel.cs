using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using rybank.Models;
using rybank.Models.Account;

namespace rybank.estudo.Models
{
    public class UserModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Senha { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public SaldoModel? Saldo { get; set; }

        public AccountModel? Dados { get; set; }

        [InverseProperty("Usuario")]
        public List<DepositModel> Deposito { get; set; } = new();

        [InverseProperty("UsuarioOrigem")]
        public List<TransferenciaModel> TransferenciasEnviadas { get; set; } = new();

        [InverseProperty("UsuarioDestino")]
        public List<TransferenciaModel> TransferenciasRecebidas { get; set; } = new();

        [InverseProperty("Usuario")]
        public List<MovimentacaoModel> Movimentacao { get; set; } = new();

        [InverseProperty("Usuario")]
        public List<BoletoModel> Boleto { get; set; } = new();

        [InverseProperty("Usuario")]
        public List<PixModel> Pix { get; set; } = new();
        
    }
}