using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;
using PersonalFinance.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinance
{
    public class BalancesModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IPersonalFinanceRepository personalFinanceRepository;
        private readonly IMapper mapper;

        public BalancesModel(UserManager<User> userManager,
            IPersonalFinanceRepository personalFinanceRepository,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.personalFinanceRepository = personalFinanceRepository;
            this.mapper = mapper;
        }


        [BindProperty]
        public TransactionDTO TransactionToAdd { get; set; }


        public Customer Customer { get; set; }
        public IEnumerable<CustomerBalance> CustomerBalances { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //This takes lots of space and actually querries DB 3 times instead of 2
            //Maybe add claim?
            await FetchCustomerData();

            return Page();
        }

        public async Task<IActionResult> OnPostOpenBalanceAsync(int balanceCurrency)
        {
            await FetchCustomerData();




            foreach (var balance in CustomerBalances)
            {
                if (balance.Currency == balanceCurrency)
                {
                    ModelState.TryAddModelError("error1", "User already has a balance in a given currency");
                    return Page();

                }
            }


            Console.WriteLine($"Customer {Customer.Email} \n Currency {balanceCurrency}");
            personalFinanceRepository.OpenBalance(Customer, balanceCurrency);

            //CustomerBalances = personalFinanceRepository.GetCustomerBalances(Customer);

            //Redirect to the same page
            return RedirectToPage("/Customers/Balances");
        }

        public IActionResult OnPostCloseBalance(int balanceId)
        {
            personalFinanceRepository.CloseBalanceById(balanceId);

            return RedirectToPage("/Customers/Balances");
        }

        public async Task<IActionResult> OnPostAddTransactionAsync()
        {
            await FetchCustomerData();
            //TransactionToAdd.Customer = Customer;
            //TransactionToAdd.Date = DateTime.Now;

            if (!ModelState.IsValid)
            {

                return Page();
            }
            //Fetch the other necessary things
            var transaction = mapper.Map<Transaction>(TransactionToAdd);
            transaction.Customer = Customer;
            transaction.Date = DateTime.Now;
            personalFinanceRepository.AddTransaction(transaction);
            return RedirectToPage("/Customers/Balances");

        }

        private async Task<User> FetchCustomerData()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            Customer = personalFinanceRepository
                .GetCustomerByIdentityId(user.Id);
            CustomerBalances = personalFinanceRepository.GetCustomerBalances(Customer);
            return user;
        }





    }
}