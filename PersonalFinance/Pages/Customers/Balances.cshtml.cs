using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;


namespace PersonalFinance
{
    public class BalancesModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IPersonalFinanceRepository personalFinanceRepository;

        public BalancesModel(UserManager<User> userManager,
            IPersonalFinanceRepository personalFinanceRepository)
        {
            this.userManager = userManager;
            this.personalFinanceRepository = personalFinanceRepository;
        }

        [BindProperty]
        public int NewBalanceCurrency { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<CustomerBalance> CustomerBalances { get; set;}
        public async Task<IActionResult> OnGetAsync()
        {
            //This takes lots of space and actually querries DB 3 times instead of 2
            //Maybe add claim?
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            Customer = personalFinanceRepository
                .GetCustomerByIdentityId(user.Id);

            CustomerBalances = personalFinanceRepository.GetCustomerBalances(Customer);

            return Page();
        }

        public async Task<IActionResult> OnPostOpenBalanceAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            Customer = personalFinanceRepository
                .GetCustomerByIdentityId(user.Id);


            Console.WriteLine($"Customer {Customer.Email} \n Currency {NewBalanceCurrency}");
            personalFinanceRepository.OpenBalance(Customer, NewBalanceCurrency);

            CustomerBalances = personalFinanceRepository.GetCustomerBalances(Customer);

            //Redirect to the same page
            return RedirectToPage("/Customers/Balances");
        }
    }
}