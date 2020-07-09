namespace Exchangerat.Requests.Services.Contracts.Requests
{
    using Exchangerat.Requests.Models.Requests;
    using Infrastructure;
    using System.Threading.Tasks;

    public interface IRequestService
    {
        Task<Result<AllRequestsOutputModel>> GetAll();

        Task<Result> Create(CreateRequestFormModel model, string userId);
    }
}
