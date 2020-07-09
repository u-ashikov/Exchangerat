namespace Exchangerat.Requests.Messages
{
    using Data.Enums;
    using Exchangerat.Messages.Admin;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using MassTransit;
    using System.Threading.Tasks;

    public class RequestApprovedConsumer : IConsumer<RequestApprovedMessage>
    {
        private readonly IRequestService requests;

        private readonly IBus publisher;

        public RequestApprovedConsumer(IRequestService requests, IBus publisher)
        {
            this.requests = requests;
            this.publisher = publisher;
        }

        public async Task Consume(ConsumeContext<RequestApprovedMessage> context)
        {
            // TODO: Refactor these strings and these if else statements!!!

            var message = context.Message;

            await this.requests.UpdateStatus(message.RequestId, Status.Approved);

            if (message.RequestType == "Create Account")
            {
                await this.publisher.Publish(new AccountCreatedMessage() { UserId = message.UserId });
            }
            else if (message.RequestType == "Block Account")
            {
                await this.publisher.Publish(new AccountBlockedMessage() { UserId = message.UserId, AccountId = message.AccountId.Value });
            }
            else if (message.RequestType == "Delete Account")
            {
                await this.publisher.Publish(new AccountDeletedMessage() { UserId = message.UserId, AccountId = message.AccountId.Value });
            }
        }
    }
}
