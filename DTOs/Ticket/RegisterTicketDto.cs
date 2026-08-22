using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.DTOs.Ticket
{
    public class RegisterTicketDto
    {
        [Required]
        public string Empresa { get; set; } = string.Empty;

        [Required]
        public string CNPJ { get; set; } = string.Empty;
        
        [Required]
        public int Vencimento { get; set; } = 30;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; } = 0.00m;
    }
}