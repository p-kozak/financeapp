using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;

namespace PersonalFinance
{
    [Authorize]
    public class AddUserModel : PageModel
    {
        private readonly IPersonalFinanceRepository repository;
        private readonly UserManager<User> userManager;
        public string Dupsko { get; set; }
        public string Idupa { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }
        public AddUserModel(IPersonalFinanceRepository repository, UserManager<User> userManager)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Dupsko = User.Identity.Name;
            var user = await userManager.FindByNameAsync(Dupsko);
            Idupa = user.Id;
            return Page();
        }

        public IActionResult OnPost()
        {
            /*PRG - post, redirect, get. Don't stay on this page after posting*/
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
       .SelectMany(v => v.Errors)
       .Select(e => e.ErrorMessage));
                Console.WriteLine(message);
                return Page();
            }
            else
            {
                repository.AddCustomer(Customer);
            }
            return RedirectToPage("/Index");
        }
    }
}