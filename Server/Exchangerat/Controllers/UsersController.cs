namespace Exchangerat.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.Contracts.Identity;
    using System.Threading.Tasks;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityService userService;

        public UsersController(IIdentityService userService)
        {
            this.userService = userService;
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] RequestModel model)
        {
            var result = await this.userService.Authenticate(model.Username, model.Password);

            if (result == null)
            {
                return this.BadRequest(new {message = "Bad Request"});
            }

            return this.Ok(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return this.Ok(this.userService.GetAll());
        }
    }
}
