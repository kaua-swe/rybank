using System.ComponentModel.DataAnnotations;

namespace rybank.Dto.Account
{
    public class UpdateDadosDto
    {

        [Required]
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }

        public string? CPF { get; set; }

        public string? PhoneNumber { get; set; }
    }
}