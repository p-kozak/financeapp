using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonalFinance.Domain
{
    public class Customer 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string IdentityId { get; set;}
        [Required, EmailAddress]
        public String Email { get; set; }
        [Required, StringLength(50)]
        public String FirstName { get; set; }
        [Required, StringLength(50)]
        public String LastName { get; set; }
        [StringLength(50)]
        public String Location { get; set; }
        [StringLength(50)]
        public String Occupation { get; set; }
    }
}
