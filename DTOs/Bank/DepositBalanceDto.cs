using System.ComponentModel.DataAnnotations;

namespace src.DTOs.Bank
{
    public class DepositBalanceDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public decimal Valor { get; set; } = 0.00m;
        
        [Required]
        public string Conta { get; set; } = string.Empty;
    }
}