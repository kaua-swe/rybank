using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using rybank.estudo.Enums;

namespace rybank.estudo.Models
{
    public class MovimentacaoModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; } = 0.00m;

        [Required]
        public TipoMovimentacaoEnum Tipo { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UsuarioId")]
        public UserModel? Usuario { get; set; }
    }
}