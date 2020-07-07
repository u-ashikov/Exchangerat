namespace Exchangerat.Admin.Controllers
{
    using Exchangerat.Admin.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class RequestsController : AdminController
    {
        private readonly IRequestService requestService;

        public RequestsController(IRequestService requestService)
        {
            this.requestService = requestService;
        }

        public async Task<IActionResult> GetAll()
        {
            var result = await this.requestService.GetAll();

            return this.View(result);
        }
    }
}
