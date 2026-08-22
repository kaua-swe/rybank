using System.ComponentModel.DataAnnotations;

namespace src.DTOs.Auth
{
    public class RegisterUserDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        public string CPF { get; set; } = string.Empty;

        [Required]
        public string Telefone { get; set; } = string.Empty;

        public string Conta { get; set; } = "CORRENTE";
    }
}