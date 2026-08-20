using System.ComponentModel.DataAnnotations;

namespace rybank.Dto.Account
{
    public class UpdateDadosDto
    {

        [Required]
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }

        [MinLength(11, ErrorMessage = "Digite um CPF válido.")]
        [MaxLength(11, ErrorMessage = "Digite um CPF válido.")]
        public string? CPF { get; set; }

        [MinLength(11, ErrorMessage = "Digite um Número válido.")]
        [MaxLength(11, ErrorMessage = "Digite um Número válido.")]       
        public string? PhoneNumber { get; set; }
    }
}