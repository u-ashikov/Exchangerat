namespace Exchangerat.Requests.Gateway.Services.Requests
{
    using Constants;
    using Exchangerat.Requests.Gateway.Models.Requests;
    using Refit;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequestService
    {
        [Get("/api/ExchangeratRequests/GetAll")]
        Task<RequestListingOutputModel> GetAll([Query]int? status, [Query]int page = WebConstants.FirstPage);

        [Get("/api/ExchangeratRequests/GetMy")]
        Task<IEnumerable<MyRequestOutputModel>> GetMy();
    }
}
