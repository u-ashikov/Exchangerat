using System.Threading.Tasks;
using Exchangerat.Admin.Models.Models.Requests;
using Refit;

namespace Exchangerat.Admin.Services.Contracts.Requests
{
    public interface IRequestService
    {
        [Get("/ExchangeratRequests/GetAll")]
        Task<AllRequestsOutputModel> GetAll();
    }
}
