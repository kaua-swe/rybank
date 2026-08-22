using System.ComponentModel.DataAnnotations;

namespace src.DTOs.Bank
{
    public class RegisterWalletDto
    {

        [Required]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Conta { get; set; } = string.Empty;
    }
}