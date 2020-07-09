namespace Exchangerat.Requests.Messages
{
    using Exchangerat.Messages.Admin;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using MassTransit;
    using System.Threading.Tasks;

    public class CreateAccountApprovedConsumer : IConsumer<RequestApprovedMessage>
    {
        private readonly IRequestService requests;

        private readonly IBus publisher;

        public CreateAccountApprovedConsumer(IRequestService requests, IBus publisher)
        {
            this.requests = requests;
            this.publisher = publisher;
        }

        public async Task Consume(ConsumeContext<RequestApprovedMessage> context)
        {
            var message = context.Message;

            await this.requests.Approve(message.RequestId);

            await this.publisher.Publish(new AccountCreatedMessage() { UserId = message.UserId });
        }
    }
}
