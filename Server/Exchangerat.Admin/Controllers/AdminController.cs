namespace Exchangerat.Admin.Controllers
{
    using Exchangerat.Admin.Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator")]
    public abstract class AdminController : Controller
    {
        protected IActionResult RedirectToHome() => this.RedirectToAction(WebConstants.Actions.Index, WebConstants.Controllers.Home);
    }
}
