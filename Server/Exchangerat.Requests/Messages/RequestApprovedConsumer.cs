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

        public RequestApprovedConsumer(IRequestService requests)
        {
            this.requests = requests;
        }

        public async Task Consume(ConsumeContext<RequestApprovedMessage> context)
        {
            await this.requests.UpdateStatus(context.Message, Status.Approved);
        }
    }
}
