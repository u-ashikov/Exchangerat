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
        public async Task<IActionResult> Login([FromBody]UserInputModel model)
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
        public async Task<IActionResult> Register([FromBody]UserInputModel model)
        {
            var result = await this.identityService.Register(model);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return await this.Login(new UserInputModel(model.UserName, model.Password));
        }
    }
}
