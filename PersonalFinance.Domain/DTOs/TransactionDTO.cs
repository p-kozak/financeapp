using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Domain.DTOs
{
    public class TransactionDTO
    {
        [Required]
        public int Currency { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
    }
}
