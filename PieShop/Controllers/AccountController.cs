using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using PieShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PieShop.Controllers
{
    
    [Authorize] // Blocking the whole controller -- only for login users
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;    // Api User Information in DB
        private readonly SignInManager<IdentityUser> _signInManager; // SignIn, Logout, etc ...

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
        }

       
            // Returning the URL redirect the user after successful login attempt.
         
        [AllowAnonymous] // Allow the non authoritize user to use this method
        public IActionResult Login (string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl 
            });

        }// end Login ()

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login (LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            // Searching for user name in the DB
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName); 

            if (user != null)
            {
                Debug.WriteLine("$$$$$$$$$$$$$$$$$$$ USER: " + user);

                string password = loginViewModel.Password; 
                Debug.WriteLine("################### Password: " + password);


                // await _signInManager.SignOutAsync(); 
                // Looking for password
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                // var result = true; 

                
                if (result.Succeeded)
                //if (result.Succeeded)
                {

                    if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                        return RedirectToAction("Index", "Home");

                    return Redirect(loginViewModel.ReturnUrl); 
                }

            }// end if 

            // Could not find the user. Retunr to the login page.
            ModelState.AddModelError("", "User Name or Password not found!");
            return View(loginViewModel); 

        }// end Login () -- HTTP POST


        [AllowAnonymous]
        public IActionResult Register ()
        {
            return View();

        }// end Register ()


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register (LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = loginViewModel.UserName };
                var result = await _userManager.CreateAsync(user, loginViewModel.Password); 

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); 
                }

            }// end if

            return View(loginViewModel);

        }// end Register ()


        [HttpPost]
        public async Task<IActionResult> Logout ()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



    }//end class 

   
}
