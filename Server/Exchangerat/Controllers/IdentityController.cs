namespace Exchangerat.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Identity;
    using Services.Contracts.Identity;
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
        [Route(nameof(Register))]
        public async Task<IActionResult> Register([FromBody]RegisterUserInputModel model)
        {
            var result = await this.identityService.Register(model);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return await this.Login(new LoginUserInputModel(model.Username, model.Password));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginUserInputModel model)
        {
            var result = await this.identityService.Login(model);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }
    }
}
