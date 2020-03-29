using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonalFinance.Domain
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int Currency { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
    }
}
