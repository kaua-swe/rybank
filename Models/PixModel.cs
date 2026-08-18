using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using rybank.Enums;
using rybank.estudo.Models;

namespace rybank.Models
{
    public class PixModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        public string Chave { get; set; } = string.Empty;

        [Required]
        public StatusPixEnum Status { get; set; } = StatusPixEnum.Pendente;

        [Required]
        public ChavePixEnum TipoChave { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }

        [ForeignKey("UsuarioId")]
        public UserModel? Usuario { get; set; }
    }
}