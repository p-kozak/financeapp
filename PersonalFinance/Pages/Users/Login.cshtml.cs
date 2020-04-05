using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalFinance.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinance
{


    public class LoginModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        [BindProperty]
        public UserLoginModel UserLoginModel { get; set; }
        public LoginModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
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

            var result = await signInManager.PasswordSignInAsync(UserLoginModel.Email, UserLoginModel.Password, UserLoginModel.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError("", "Login failed");
                return Page();
            }
            /*
             //Use signinmanager to automate this piece of code
            var user = await userManager.FindByEmailAsync(UserLoginModel.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, UserLoginModel.Password))
            {
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(identity));

                return RedirectToPage("/Index");
            }

            else
            {
                ModelState.AddModelError("", "Login failed");
                return Page();
            }
            */

        }



        //Action for logout

    }
}