using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Domain
{
    public class BalanceHistory
    {
        //This ID is unnecessary but I cannot find different solution. EF core required primary key
        [Key]
        public int Id { get; set; }
        [Required]
        public CustomerBalance TrackedBalance { get; set; }
        [Required]
        public Customer Customer { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int Currency { get; set; }

    }
}
