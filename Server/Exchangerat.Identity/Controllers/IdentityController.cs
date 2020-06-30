namespace Exchangerat.Identity.Controllers
{
    using Exchangerat.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.Contracts;
    using System.Threading.Tasks;

    public class IdentityController : BaseApiController
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login([FromBody]LoginUserInputModel model)
        {
            var result = await this.identityService.Login(model);

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
            var result = await this.identityService.Register(model);

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
            var userIds = await this.identityService.GetRegisterUsersIds();

            return this.Ok(userIds);
        }
    }
}
