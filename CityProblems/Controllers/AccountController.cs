using CityProblems.Models;
using CityProblems.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

namespace CityProblems.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string username, string password)
        {
            var user = await _userService.Get(username, password);

            if(user is not null)
            {
                var isSuccess = await _userService.Authorize(user, HttpContext);

                if (isSuccess)
                {
					return Redirect("/Home/Index");
				}

                return Redirect("/Account/SignIn");
            }

            return PartialView(new Models.AccountViewModel
            {
                signInModel = new Models.SignInModel
                {
                    Username = username,
                    Password = password
                },

                Error = "Пароль или имя пользователя введены неверно! Попробуйте еще раз."
            });
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return PartialView(new AccountViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string username, string password, string email)
        {
            var user = await _userService.Create(username, password, email);

            if(user is not null)
            {
				var isSuccess = await _userService.Authorize(user, HttpContext);

				if (isSuccess)
				{
					return Redirect("/Home/Index");
				}

				return Redirect("/Account/SignIn");
			}

            return PartialView(new Models.AccountViewModel
            {
                signUpModel = new Models.SignUpModel
                {
                    Username = username,
                    Password = password,
                    Email = email
                },

                Error = "Это имя пользователя или почта заняты, попробуйте другое!"
            });
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return PartialView(new AccountViewModel());
        }

        [Authorize]
        public async Task<IActionResult> Quit()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Home/Index");
        }
    }
}
