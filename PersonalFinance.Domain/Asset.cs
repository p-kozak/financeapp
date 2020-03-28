using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonalFinance.Domain
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public int Type { get; set; }
        public int Currency { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public DateTime EndOfOwnerShipDate { get; set; }
        /*If the asset is active or already sols*/
        [Required]
        public bool Active { get; set; }

    }
}
