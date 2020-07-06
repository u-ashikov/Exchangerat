namespace Exchangerat.Admin.Controllers
{
    using Exchangerat.Admin.Common.Constants;
    using Exchangerat.Admin.Models.Models.Identity;
    using Exchangerat.Admin.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Refit;
    using System;
    using System.Threading.Tasks;

    public class IdentityController : AdminController
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [AllowAnonymous]
        public IActionResult Login() => this.View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var result = await this.identityService.Login(model, true);

                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    this.Response.Cookies.Append(WebConstants.AuthCookieKey, result.Token, new CookieOptions()
                    {
                        HttpOnly = true,
                        //Secure = true,
                        MaxAge = TimeSpan.FromDays(1)
                    });

                    return this.RedirectToHome();
                }

                this.ModelState.AddModelError(string.Empty, WebConstants.Messages.GeneralError);

                return this.View(model);
            }
            catch (ApiException ex)
            {
                this.ModelState.AddModelError(string.Empty, string.Join(Environment.NewLine, ex.Content.Split(",", StringSplitOptions.RemoveEmptyEntries)));

                return this.View(model);
            }
        }

        public IActionResult Logout()
        {
            this.Response.Cookies.Delete(WebConstants.AuthCookieKey);

            return this.RedirectToHome();
        }
    }
}
