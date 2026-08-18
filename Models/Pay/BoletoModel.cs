using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using rybank.estudo.Enums;

namespace rybank.estudo.Models
{
    public class BoletoModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        public Guid DevedorId { get; set; }

        [Required]
        public string Empresa { get; set; } = string.Empty;

        [Required]
        public decimal ValorBoleto { get; set; } = 0.00m;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        public DateTime Maturity { get; set; }

        public DateTime? PayedAt { get; set; }

        public StatusBoletoEnum Status { get; set; } = StatusBoletoEnum.Pendente;

        [ForeignKey("DevedorId")]
        public UserModel? Usuario { get; set; }
    }
}