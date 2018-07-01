using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SilentLux.Model;
using SilentLux.Services.Interfaces;
using SilentLux.Web.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SilentLux.Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [Route("signin")]
        public IActionResult SignIn()
        {
            return View(new SignInModel());
        }

        [Route("signin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.ValidateCredentials(model.Username, model.Password, out User user))
                {
                    await SignInUser(user.Username);

                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [Route("signout")]
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [Route("signup")]
        public IActionResult SignUp()
        {
            return View(new SignUpModel());
        }

        [Route("signup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.AddUser(model.Username, model.Password))
                {
                    await SignInUser(model.Username);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Error", "Could not add user, username already in use.");
            }

            return View(model);
        }

        public async Task SignInUser(string username)
        {
            var nameClaimType = "name";

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(nameClaimType, username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, nameClaimType, null);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }
    }
}