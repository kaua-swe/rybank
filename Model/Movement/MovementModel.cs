using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Model.Auth;

namespace src.Model.Movement
{
    public class MovementModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        public string Conta { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UsuarioId")]
        public UserModel? Usuario { get; set; }

    }
}