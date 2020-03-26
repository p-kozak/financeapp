using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;

namespace PersonalFinance.Pages.Customers
{
    public class TransactionsModel : PageModel
    {
        private readonly IPersonalFinanceRepository personalFinanceRepository;
        private readonly UserManager<User> userManager;

        public TransactionsModel(IPersonalFinanceRepository personalFinanceRepository,
            UserManager<User> userManager)
        {
            this.personalFinanceRepository = personalFinanceRepository;
            this.userManager = userManager;
        }

        public Customer Customer { get; set; }
        public ICollection<Transaction> CustomerTransactions { get; set; }


        //Searchig properties
        public string DateSort { get; set; }
        public string AmountSort { get; set; }
        public string CurrencySort { get; set; }
        public string DescriptionSort { get; set; }

        private async Task<User> FetchCustomerData()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            Customer = personalFinanceRepository
                .GetCustomerByIdentityId(user.Id);
            CustomerTransactions = personalFinanceRepository.GetCustomerTransactions(Customer);
            return user;
        }
        public async Task<IActionResult> OnGetAsync(string sortString, string filterString)
        {
            await FetchCustomerData();

            if (!string.IsNullOrEmpty(filterString))
            {
                CustomerTransactions = CustomerTransactions.Where(s => s.Date.ToString().Contains(filterString) ||
                s.Amount.ToString().Contains(filterString) ||
                s.Currency.ToString().Contains(filterString) ||
                s.Description.ToString().Contains(filterString)
                ).ToList();
            }

            DateSort = string.IsNullOrEmpty(sortString) ? "Date_Dsc_Sort" : "";
            CurrencySort = sortString == "Currency_Asc_Sort" ? "Currency_Dsc_Sort" : "Currency_Dsc_Sort";
            AmountSort = sortString == "Amount_Asc_Sort" ? "Amount_Dsc_Sort" : "Amount_Asc_Sort";
            DescriptionSort = sortString == "Description_Asc_Sort" ? "Description_Dsc_Sort" : "Description_Dsc_Sort";


            switch (sortString)
            {
                case "Date_Dsc_Sort":
                    CustomerTransactions = CustomerTransactions.OrderByDescending(s => s.Date).ToList();
                    break;
                case "Currency_Asc_Sort":
                    CustomerTransactions = CustomerTransactions.OrderBy(s => s.Currency).ToList();
                    break;
                case "Currency_Dsc_Sort":
                    CustomerTransactions = CustomerTransactions.OrderByDescending(s => s.Currency).ToList();
                    break;
                case "Amount_Asc_Sort":
                    CustomerTransactions = CustomerTransactions.OrderBy(s => s.Amount).ToList();
                    break;
                case "Amount_Dsc_Sort":
                    CustomerTransactions = CustomerTransactions.OrderByDescending(s => s.Amount).ToList();
                    break;
                case "Description_Asc_Sort":
                    CustomerTransactions = CustomerTransactions.OrderBy(s => s.Description).ToList();
                    break;
                case "Description_Dsc_Sort":
                    CustomerTransactions = CustomerTransactions.OrderByDescending(s => s.Description).ToList();
                    break;



            }


            return Page();


        }
    }
}