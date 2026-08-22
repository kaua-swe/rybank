using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Model.Auth;

namespace src.Model.Bank
{
    public class BalanceModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        [Required]
        public string Conta { get; set; } = string.Empty;

        [Required]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("UsuarioId")]
        public UserModel? Usuario { get; set; }
    }
}