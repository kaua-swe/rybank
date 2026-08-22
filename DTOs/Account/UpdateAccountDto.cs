using System.ComponentModel.DataAnnotations;

namespace src.DTOs.Account
{
    public class UpdateAccountDto
    {

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        public string CPF { get; set; } = string.Empty;

        [Required]
        public string Telefone { get; set; } = string.Empty;

    }
}