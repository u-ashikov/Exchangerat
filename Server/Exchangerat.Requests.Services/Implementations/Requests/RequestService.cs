namespace Exchangerat.Requests.Services.Implementations.Requests
{
    using Constants;
    using Contracts.ExchangeAccounts;
    using Data;
    using Data.Enums;
    using Exchangerat.Requests.Common.Constants;
    using Exchangerat.Requests.Data.Models;
    using Exchangerat.Requests.Models.Models.Requests;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class RequestService : IRequestService
    {
        private readonly RequestsDbContext dbContext;

        private readonly IExchangeAccountService exchangeAccounts;

        public RequestService(RequestsDbContext dbContext, IExchangeAccountService exchangeAccounts)
        {
            this.dbContext = dbContext;
            this.exchangeAccounts = exchangeAccounts;
        }

        public async Task<Result<AllRequestsOutputModel>> GetAll()
        {
            var result = new AllRequestsOutputModel();

            var requests = await this.dbContext.ExchangeratRequests
                .Select(er => new RequestOutputModel() 
                {
                   Id = er.Id,
                   RequestType = er.RequestType.Type,
                   UserId = er.UserId,
                   AccountId = er.AccountId,
                   Status = er.Status.ToString(),
                   IssuedAt = er.IssuedAt
                })
                .AsNoTracking()
                .ToListAsync();

            result.Requests = requests;

            return Result<AllRequestsOutputModel>.SuccessWith(result);
        }

        public async Task<Result> Create(CreateRequestFormModel model, string userId)
        {
            var createAccountRequestType =
                this.dbContext.RequestTypes.FirstOrDefault(rt => rt.Type == "Create Account");

            if (createAccountRequestType == null)
            {
                return Result.Failure(WebConstants.DefaultErrorMessage);
            }

            if (model.RequestTypeId != createAccountRequestType.Id && !model.AccountId.HasValue)
            {
                return Result.Failure(ServiceConstants.AccountDoesNotExist);
            }

            var request = new ExchangeratRequest()
            {
                RequestTypeId = model.RequestTypeId,
                Status = Status.Pending,
                UserId = userId,
                IssuedAt = DateTime.UtcNow
            };

            if (model.RequestTypeId != createAccountRequestType.Id)
            {
                var isAccountOwner = await this.exchangeAccounts.IsOwner(model.AccountId.Value, userId);

                if (!isAccountOwner)
                {
                    return Result.Failure(ServiceConstants.AccountDoesNotExist);
                }

                request.AccountId = model.AccountId;
            }

            this.dbContext.ExchangeratRequests.Add(request);

            await this.dbContext.SaveChangesAsync();

            return Result.Success;
        }

        public async Task Approve(int requestId)
        {
            var request = await this.dbContext.ExchangeratRequests.FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null || request.Status != Status.Pending)
            {
                return;
            }

            request.Status = Status.Approved;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
