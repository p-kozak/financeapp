using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;
using PersonalFinance.Domain.DTOs;
using PersonalFinance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinance
{
    public class BalancesModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IPersonalFinanceRepository personalFinanceRepository;
        private readonly IMapper mapper;
        private readonly ICurrencyConverter currencyConverter;

        public BalancesModel(UserManager<User> userManager,
            IPersonalFinanceRepository personalFinanceRepository,
            IMapper mapper,
            ICurrencyConverter currencyConverter)
        {
            this.userManager = userManager;
            this.personalFinanceRepository = personalFinanceRepository;
            this.mapper = mapper;
            this.currencyConverter = currencyConverter;
        }


        [BindProperty]
        public TransactionDTO TransactionToAdd { get; set; }


        public Customer Customer { get; set; }
        public IEnumerable<CustomerBalance> CustomerBalances { get; set; }
        public decimal ConvertedBalance { get; set; }
        public int ConvertedCurrency { get; set; }
        public async Task<IActionResult> OnGetAsync(int currencyToConvert)
        {
            //This takes lots of space and actually querries DB 3 times instead of 2
            //Maybe add claim?
            await FetchCustomerData();

            //Check if we have currency like this in the list of balances.
            if(CustomerBalances.Where(o => o.Currency == currencyToConvert).Count() > 0 )
            {
                var convertedBalance = currencyConverter.ConvertListOfBalancesToGivenCurrency(CustomerBalances, (Currency)currencyToConvert);
                ConvertedBalance = convertedBalance;
                ConvertedCurrency = currencyToConvert;
            }

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
            //var convertedBalance = currencyConverter.ConvertListOfBalancesToGivenCurrency(CustomerBalances, Currency.PLN);
            //ConvertedBalance = convertedBalance;
            return user;
        }





    }
}