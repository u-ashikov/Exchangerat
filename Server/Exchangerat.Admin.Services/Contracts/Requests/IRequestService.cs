namespace Exchangerat.Admin.Services.Contracts.Requests
{
    using Exchangerat.Admin.Models.Requests;
    using Refit;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequestService
    {
        [Get("/Requests/GetAll")]
        Task<IEnumerable<RequestViewModel>> GetAll();
    }
}
