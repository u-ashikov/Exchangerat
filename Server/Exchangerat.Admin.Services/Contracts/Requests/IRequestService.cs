namespace Exchangerat.Admin.Services.Contracts.Requests
{
    using Constants;
    using Exchangerat.Admin.Models.Requests;
    using Refit;
    using System.Threading.Tasks;

    public interface IRequestService
    {
        [Get("/api/Requests/GetAll")]
        Task<AllRequestsViewModel> GetAll([Query]int? status, [Query]int page = WebConstants.FirstPage);
    }
}
