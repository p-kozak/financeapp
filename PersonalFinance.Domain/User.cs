using Microsoft.AspNetCore.Identity;

namespace PersonalFinance.Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
