using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinance
{
    public class RegisterModel : PageModel
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IPersonalFinanceRepository personalFinanceRepository;
        private readonly SignInManager<User> signInManager;

        [BindProperty]
        public UserRegistrationModel UserToRegister { get; set; }


        //Dependency injection
        public RegisterModel(IMapper mapper, UserManager<User> userManager,
            IPersonalFinanceRepository personalFinanceRepository, SignInManager<User> signInManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.personalFinanceRepository = personalFinanceRepository;
            this.signInManager = signInManager;
        }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                Console.WriteLine(message);
                return Page();
            }
            var user = mapper.Map<User>(UserToRegister);

            var result = await userManager.CreateAsync(user, UserToRegister.Password);
            if (result.Succeeded)
            {
                var customer = new Customer
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IdentityId = user.Id,
                    Email = user.Email
                };


                //Add a customer as well
                personalFinanceRepository.AddCustomer(customer);

                await userManager.AddToRoleAsync(user, "Customer");
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return Page();
            }

            return RedirectToPage("/Index");

        }
    }
}