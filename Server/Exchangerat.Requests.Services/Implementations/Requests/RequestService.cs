﻿namespace Exchangerat.Requests.Services.Implementations.Requests
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

        public async Task<Result<AllRequestsOutputModel>> GetAll(Status? status, int page = WebConstants.FirstPage)
        {
            var requests = await this.All()
                .Select(er => new RequestOutputModel() 
                {
                   Id = er.Id,
                   RequestType = er.RequestType.Type,
                   UserId = er.UserId,
                   AccountId = er.AccountId,
                   AccountTypeId = er.AccountTypeId,
                   Status = er.Status.ToString(),
                   IssuedAt = er.IssuedAt
                })
                .AsNoTracking()
                .ToListAsync();

            var totalItems = await this.TotalRequests(status);
            var totalPages = (int) Math.Ceiling(totalItems / (double)WebConstants.ItemsPerPage);

            if (page < WebConstants.FirstPage)
            {
                page = WebConstants.FirstPage;
            }

            if (page > totalPages)
            {
                page = totalPages;
            }

            if (status.HasValue)
            {
                requests = requests
                    .Where(er => er.Status == status.Value.ToString())
                    .ToList();
            }

            var result = new AllRequestsOutputModel()
            {
                Requests = requests.Skip((page - WebConstants.FirstPage) * WebConstants.ItemsPerPage)
                    .Take(WebConstants.ItemsPerPage)
                    .ToList(),
                TotalItems = totalItems
            };

            return Result<AllRequestsOutputModel>.SuccessWith(result);
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
            var createAccountRequestType = this.dbContext.Set<RequestType>().FirstOrDefault(rt => rt.Type == "Create Account");

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
                AccountTypeId = model.AccountTypeId,
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

        public async Task UpdateStatus<TMessage>(TMessage message, Status status)
            where TMessage : BaseRequestStatusUpdatedMessage
        {
            var request = await this.All().FirstOrDefaultAsync(r => r.Id == message.RequestId);

            if (request == null || request.Status != Status.Pending)
            {
                return;
            }

            if (status == Status.Approved)
            {
                await this.Approve(message, request);
            }
            else if (status == Status.Cancelled)
            {
                await this.Cancel(request);
            }
        }

        public async Task<int> TotalRequests(Status? status)
        {
            if (status.HasValue)
            {
                return await this.All().CountAsync(er => er.Status == status);
            }

            return await this.All().CountAsync();
        }

        private async Task Approve(BaseRequestStatusUpdatedMessage message, ExchangeratRequest request)
        {
            if (message == null || request == null)
            {
                throw new ArgumentNullException();
            }

            request.Status = Status.Approved;
            object messageData = null;

            if (message.RequestType == "Create Account")
            {
                messageData = new AccountCreatedMessage()
                {
                    UserId = message.UserId,
                    AccountTypeId = message.AccountTypeId.Value
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
            else if (message.RequestType == "Close Account")
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

        private async Task Cancel(ExchangeratRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Status = Status.Cancelled;

            await this.Save(request);
        }
    }
}
