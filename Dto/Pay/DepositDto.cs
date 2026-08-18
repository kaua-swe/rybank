using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace rybank.estudo.Dto
{
    public class DepositDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; } = 0.00m;
    }
}