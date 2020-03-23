using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonalFinance.Domain
{
    public class BalanceHistory
    {
        //This ID is unnecessary but I cannot find different solution. EF core required primary key
        public int Id { get; set; }
        [Required]
        public Customer Customer { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public decimal Balance { get; set;}
        [Required]
        public int Currency { get; set; }

    }
}
