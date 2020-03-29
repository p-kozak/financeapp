using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;
using PersonalFinance.Domain.DTOs;

namespace PersonalFinance.Pages.Customers
{
    public class EditTransactionModel : PageModel
    {
        private readonly IPersonalFinanceRepository personalFinanceRepository;
        private readonly UserManager<User> userManager;

        public Customer Customer { get; set; }
        public Transaction Transaction { get; set; }
        [BindProperty]
        public TransactionDTO TransactionDTO { get; set; }
        [BindProperty]
        public int TransactionId { get; set; }

        public EditTransactionModel(IPersonalFinanceRepository personalFinanceRepository,
            UserManager<User> userManager)
        {
            this.personalFinanceRepository = personalFinanceRepository;
            this.userManager = userManager;
        }

        private async Task<User> FetchCustomerData()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            Customer = personalFinanceRepository
                .GetCustomerByIdentityId(user.Id);
            return user;
        }



        public async Task<IActionResult> OnGetAsync(string transId)
        {
            Console.WriteLine($"{transId} on get");
            if (string.IsNullOrEmpty(transId))
            {
                return RedirectToPage("/Customers/Transactions");
            }

            await FetchCustomerData();

            var intId = int.Parse(transId);
            var transaction = personalFinanceRepository.GetTransactionById(intId);

            if(transaction.Customer.IdentityId != Customer.IdentityId)
            {
                return RedirectToPage("/Customers/Transactions");
            }
            Transaction = transaction;
            return Page();

        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Console.WriteLine($"Getting by id {TransactionId}");

            Console.WriteLine($"DTO is {TransactionDTO.Amount}");
            Console.WriteLine($"DTO is {TransactionDTO.Currency}");
            Console.WriteLine($"DTO is {TransactionDTO.Description}");




            var transaction = personalFinanceRepository.GetTransactionById(TransactionId);
            Console.WriteLine($"I got {transaction.Id}");
            Console.WriteLine($"I got {transaction.Description}");
            Console.WriteLine($"I got {transaction.Currency}");
            Console.WriteLine($"I got {transaction.Amount}");
            Console.WriteLine($"I got customer {transaction.Customer.Id}");
            Console.WriteLine($"I got customer {transaction.Customer.Id}");
            Console.WriteLine($"I got customer {transaction.Customer.Email}");

            Console.WriteLine($"I got customer {transaction.Customer.FirstName}");

            Console.WriteLine($"I got customer {transaction.Customer.LastName}");






            Console.WriteLine($"Is user null? {(transaction.Customer == null).ToString()}");
            personalFinanceRepository.EditTransaction(transaction, TransactionDTO);
            return RedirectToPage("/Customers/Transactions");
        }
       

        
        


    }
}