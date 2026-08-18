using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using rybank.estudo.Models;

namespace rybank.Models.Account
{
    public class AccountModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UsuarioId { get; set;}

        public string? DisplayName { get; set; }

        public string? CPF { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UsuarioId")]
        public UserModel? Usuario { get; set; }
    }
}