﻿namespace Exchangerat.Requests.Messages
{
    using Data.Enums;
    using Exchangerat.Messages.Admin;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using MassTransit;
    using System.Threading.Tasks;

    public class RequestCancelledConsumer : IConsumer<RequestCancelledMessage>
    {
        private readonly IRequestService requests;

        public RequestCancelledConsumer(IRequestService requests)
        {
            this.requests = requests;
        }

        public async Task Consume(ConsumeContext<RequestCancelledMessage> context)
        {
            await this.requests.UpdateStatus(context.Message, Status.Cancelled);
        }
    }
}
