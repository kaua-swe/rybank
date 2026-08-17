using System.ComponentModel.DataAnnotations;


namespace rybank.estudo.Dto
{
    public class RegisterDto
    {

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}