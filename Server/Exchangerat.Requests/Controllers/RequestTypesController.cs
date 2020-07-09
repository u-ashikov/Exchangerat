namespace Exchangerat.Requests.Controllers
{
    using Exchangerat.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.RequestTypes;
    using System.Threading.Tasks;

    public class RequestTypesController : BaseApiController
    {
        private readonly IRequestTypeService requestTypes;

        public RequestTypesController(IRequestTypeService requestTypes)
        {
            this.requestTypes = requestTypes;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
            => this.Ok(await this.requestTypes.GetAll());
    }
}
