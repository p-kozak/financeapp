using System.ComponentModel.DataAnnotations;

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
