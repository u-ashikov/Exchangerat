namespace Exchangerat.Requests.Services.Contracts.Requests
{
    using Constants;
    using Data.Enums;
    using Exchangerat.Requests.Data.Models;
    using Exchangerat.Requests.Models.Models.Requests;
    using Exchangerat.Services.Common;
    using Infrastructure;
    using Messages.Admin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequestService : IDataService<ExchangeratRequest>
    {
        Task<Result<AllRequestsOutputModel>> GetAll(Status? status, int page = WebConstants.FirstPage);

        Task<Result<IEnumerable<RequestOutputModel>>> GetMy(string userId);

        Task<Result> Create(CreateRequestFormModel model, string userId);

        Task UpdateStatus<TMessage>(TMessage message, Status status)
            where TMessage : BaseRequestStatusUpdatedMessage;

        Task<int> TotalRequests(Status? status);
    }
}
