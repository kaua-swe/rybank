using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rybank.estudo.Models
{
    public class SaldoModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; } = 0.00m;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UsuarioId")]
        public UserModel? Usuario { get; set; }

    }
}