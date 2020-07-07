namespace Exchangerat.Admin.Services.Contracts
{
    using Exchangerat.Admin.Models.Models.Requests;
    using Refit;
    using System.Threading.Tasks;

    public interface IRequestService
    {
        [Get("/ExchangeratRequests/GetAll")]
        Task<AllRequestsOutputModel> GetAll();
    }
}
