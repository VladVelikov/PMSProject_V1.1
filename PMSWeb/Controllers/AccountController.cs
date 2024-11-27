using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PMS.Data.Models.Identity;
using PMSWeb.ViewModels.CommonVM;
using System.Security.Claims;

namespace PMSWeb.Controllers
{
    /// <summary>
    /// Below controller will not be reworked to services, because all the business logic is using the Identity services anyway. 
    /// </summary>
   
    public class AccountController(SignInManager<PMSUser> signInManager, UserManager<PMSUser> userManager) : BasicController
    {
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            /// Check if user already exists
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                /// Redirect to Complete the Registration
                return RedirectToAction(nameof(CompleteRegistration), new { email, returnUrl });
            }

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> CompleteRegistration(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Dashboard", "Home");
            }

            var model = new ExternalRegistrationViewModel()
            {
                Email = email
            };
            return View(new ExternalRegistrationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CompleteRegistration(ExternalRegistrationViewModel model)
        {
            var existingUser = await userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("UserName", "This Username Already Exist , Please choose another one!");
                return View(model);
            }

            if (!ModelState.IsValid || model.Position == null)
            {
                ModelState.AddModelError("UserName", "Data entered is not valid. Please enter correct data for UserName and Position!");
                return View(model);
            }


            var user = new PMSUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Position = model.Position
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                //below is to complete the role assignment
                await userManager.AddToRoleAsync(user, user.Position);

                var info = await signInManager.GetExternalLoginInfoAsync();
                if (info != null)
                {
                    await userManager.AddLoginAsync(user, info);
                }

                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Dashboard", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("UserName", "Unexpected error!");
            }

            return View(model);
        }
    }
}
