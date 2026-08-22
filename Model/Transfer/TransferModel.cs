using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Model.Auth;

namespace src.Model.Trasnfer
{
    public class TransferModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrigemUserId { get; set; }
        
        [Required]
        public Guid DestinoUserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; } = 0.00m;

        [Required]
        public DateTime CreatedAt { get; set;} = DateTime.UtcNow;

        [ForeignKey("OrigemUserId")]
        public UserModel? OrigemUser { get; set; }

        [ForeignKey("DestinoUserId")]
        public UserModel? DestinoUser { get; set; }
    }
}