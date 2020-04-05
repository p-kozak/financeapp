using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Domain;
using System;
using System.Threading.Tasks;

namespace PersonalFinance.Controllers
{
    public class LogoutController : Controller
    {
        private readonly SignInManager<User> signInManager;

        public LogoutController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Console.WriteLine("SRANIE");
            await signInManager.SignOutAsync();

            return RedirectToPage("/Index");
        }

        [HttpGet]
        public string Welcome()
        {
            return "This is the Welcome action method...";
        }
    }
}