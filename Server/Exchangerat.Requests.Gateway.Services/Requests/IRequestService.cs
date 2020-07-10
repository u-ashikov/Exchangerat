namespace Exchangerat.Requests.Gateway.Services.Requests
{
    using Exchangerat.Requests.Gateway.Models.Requests;
    using Refit;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequestService
    {
        [Get("/ExchangeratRequests/GetAll")]
        Task<IEnumerable<RequestOutputModel>> GetAll();
    }
}
