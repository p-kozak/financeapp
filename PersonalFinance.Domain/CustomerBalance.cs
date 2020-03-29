using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonalFinance.Domain
{
    public class CustomerBalance
    {
        [Key]
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public int Currency { get; set; }
        public decimal Balance { get; set; }




    }
}
