namespace Exchangerat.Admin.Controllers
{
    using Exchangerat.Admin.Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.Identity;
    using Refit;
    using Services.Contracts.Identity;
    using System;
    using System.Threading.Tasks;

    public class IdentityController : AdminController
    {
        private readonly IIdentityService identity;

        public IdentityController(IIdentityService identity)
        {
            this.identity = identity;
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
                var result = await this.identity.Login(model, true);

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
                this.ModelState.AddModelError(string.Empty, string.Join(Environment.NewLine, ex.Content.Split(new string[] { ",", "\"", "[", "]"}, StringSplitOptions.RemoveEmptyEntries)));

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
