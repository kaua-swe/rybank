using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Model.Auth;

namespace src.Model.Bank
{
    public class WalletModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UsuarioId { get; set; }
        
        [Required]
        public string Conta { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("UsuarioId")]
        public UserModel? Usuario { get; set; }
    }
}