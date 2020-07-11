namespace Exchangerat.Identity.Controllers
{
    using Exchangerat.Controllers;
    using Exchangerat.Identity.Models.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System.Threading.Tasks;

    public class IdentityController : BaseApiController
    {
        private readonly IIdentityService identity;

        public IdentityController(IIdentityService identity)
        {
            this.identity = identity;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login([FromBody]LoginUserInputModel model, [FromQuery]bool adminLogin = false)
        {
            var result = await this.identity.Login(model, adminLogin);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register([FromBody]RegisterUserInputModel model)
        {
            var result = await this.identity.Register(model);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return await this.Login(new LoginUserInputModel(model.UserName, model.Password));
        }

        // TODO: Only for seeding purposes. It must be deleted after successful seed.
        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(GetRegisteredUsersIds))]
        public async Task<IActionResult> GetRegisteredUsersIds()
        {
            var userIds = await this.identity.GetRegisterUsersIds();

            return this.Ok(userIds);
        }
    }
}
