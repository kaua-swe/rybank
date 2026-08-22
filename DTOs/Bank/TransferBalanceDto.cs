using System.ComponentModel.DataAnnotations.Schema;

namespace src.DTOs.Bank
{
    public class TransferBalanceDto
    {
        public string Email { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty;

        public string Chave { get; set; } = string.Empty;

        public string Conta { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; } = 0.00m;
    }
}