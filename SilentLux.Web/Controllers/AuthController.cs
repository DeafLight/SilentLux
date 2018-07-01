using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Pluralsight.AspNetCore.Auth.Web;
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
        public async Task<IActionResult> SignIn()
        {
            var authResult = await HttpContext.AuthenticateAsync(Startup.TemporaryScheme);

            if (authResult.Succeeded)
            {
                return RedirectToAction(nameof(Profile));
            }

            return View();
        }

        [Route("signin/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl = null)
        {
            var redirectUri = Url.Action(nameof(Profile));

            if (returnUrl != null)
            {
                redirectUri += $"?returnUrl={returnUrl}";
            }

            return Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, provider);
        }

        [Route("signin-local")]
        public IActionResult SignInLocal()
        {
            return View(new SignInModel());
        }

        [Route("signin-local")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignInLocal(SignInModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.ValidateCredentialsAsync(model.Username, model.Password, out LocalUser user))
                {
                    await SignInUser(user.Id);

                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [Route("signin/profile")]
        public async Task<IActionResult> Profile(string returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync(Startup.TemporaryScheme);

            if (!authResult.Succeeded)
            {
                return RedirectToAction(nameof(SignIn));
            }

            var user = await _userService.GetUserByIdAsync(authResult.Principal.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (user != null)
            {
                return await SignInUser(user, returnUrl);
            }

            var model = new ProfileModel
            {
                DisplayName = authResult.Principal.Identity.Name
            };

            var emailClaim = authResult.Principal.FindFirst(ClaimTypes.Email);

            if (emailClaim != null)
            {
                model.Email = emailClaim.Value;
            }

            return View(model);
        }

        [Route("signin/profile")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileModel model, string returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync(Startup.TemporaryScheme);

            if (!authResult.Succeeded)
            {
                return RedirectToAction(nameof(SignIn));
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.AddSocialUserAsync(authResult.Principal.FindFirst(ClaimTypes.NameIdentifier).Value, model.DisplayName, model.Email);
                return await SignInUser(user, returnUrl);
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
                if (await _userService.AddLocalUserAsync(model.Id, model.Password, model.DisplayName, model.Email))
                {
                    await SignInUser(model.Id);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Error", "Could not add user, username already in use.");
            }

            return View(model);
        }

        public async Task SignInUser(string username)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private async Task<IActionResult> SignInUser(IUser user, string returnUrl = null)
        {
            await HttpContext.SignOutAsync(Startup.TemporaryScheme);

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect(returnUrl ?? "/");
        }
    }
}