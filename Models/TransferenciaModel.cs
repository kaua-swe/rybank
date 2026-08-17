using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rybank.estudo.Models
{
    public class TransferenciaModel
    {
        
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrigemUserId { get; set; }

        [Required]
        public Guid DestinoUserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; } = 0.00m;
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("OrigemUserId")]
        public UserModel? UsuarioOrigem { get; set; }

        [ForeignKey("DestinoUserId")]
        public UserModel? UsuarioDestino { get; set; }

    }
}