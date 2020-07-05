namespace Exchangerat.Requests.Services.Contracts.Requests
{
    using Exchangerat.Infrastructure;
    using Exchangerat.Requests.Models.Requests;
    using System.Threading.Tasks;

    public interface IRequestService
    {
        Task<Result<AllRequestsOutputModel>> GetAll();
    }
}
