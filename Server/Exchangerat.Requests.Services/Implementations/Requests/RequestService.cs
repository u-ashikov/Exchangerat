using Exchangerat.Requests.Services.Contracts.RequestTypes;

namespace Exchangerat.Requests.Services.Implementations.Requests
{
    using Constants;
    using Contracts.ExchangeAccounts;
    using Data;
    using Data.Enums;
    using Exchangerat.Data.Models;
    using Exchangerat.Requests.Common.Constants;
    using Exchangerat.Requests.Data.Models;
    using Exchangerat.Requests.Models.Models.Requests;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using Exchangerat.Services.Common;
    using Infrastructure;
    using MassTransit;
    using Messages.Admin;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RequestService : DataService<ExchangeratRequest>, IRequestService
    {
        private readonly IExchangeAccountService exchangeAccounts;

        private readonly IBus publisher;

        public RequestService(RequestsDbContext dbContext, IExchangeAccountService exchangeAccounts, IBus publisher) 
            : base(dbContext)
        {
            this.exchangeAccounts = exchangeAccounts;
            this.publisher = publisher;
        }

        public async Task<Result<IEnumerable<RequestOutputModel>>> GetAll()
        {
            var requests = await this.All()
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

            return Result<IEnumerable<RequestOutputModel>>.SuccessWith(requests);
        }

        public async Task<Result<IEnumerable<RequestOutputModel>>> GetMy(string userId)
        {
            var requests = await this.All()
                .Where(r => r.UserId == userId)
                .AsNoTracking()
                .Select(r => new RequestOutputModel()
                {
                    Id = r.Id,
                    AccountId = r.AccountId,
                    RequestType = r.RequestType.Type,
                    Status = r.Status.ToString(),
                    UserId = r.UserId,
                    IssuedAt = r.IssuedAt
                })
                .ToListAsync();

            return Result<IEnumerable<RequestOutputModel>>.SuccessWith(requests);
        }

        public async Task<Result> Create(CreateRequestFormModel model, string userId)
        {
            var createAccountRequestType =
                this.dbContext.Set<RequestType>().FirstOrDefault(rt => rt.Type == "Create Account");

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

            this.dbContext.Add(request);

            await this.dbContext.SaveChangesAsync();

            return Result.Success;
        }

        public async Task UpdateStatus(RequestApprovedMessage message, Status status)
        {
            var request = await this.All().FirstOrDefaultAsync(r => r.Id == message.RequestId);

            if (request == null || request.Status != Status.Pending)
            {
                return;
            }

            request.Status = status;
            object messageData = null;

            if (message.RequestType == "Create Account")
            {
                messageData = new AccountCreatedMessage()
                {
                    UserId = message.UserId
                };
            }
            else if (message.RequestType == "Block Account")
            {
                messageData = new AccountBlockedMessage()
                {
                    UserId = message.UserId,
                    AccountId = message.AccountId.Value
                };
            }
            else if (message.RequestType == "Delete Account")
            {
                messageData = new AccountDeletedMessage()
                {
                    UserId = message.UserId,
                    AccountId = message.AccountId.Value
                };
            }

            var messageToStore = new Message(messageData);

            await this.Save(request, messageToStore);

            await this.publisher.Publish(messageData);

            await this.MarkMessageAsPublished(messageToStore.Id);
        }
    }
}
