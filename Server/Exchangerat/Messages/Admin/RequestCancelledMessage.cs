namespace Exchangerat.Messages.Admin
{
    public class RequestCancelledMessage : BaseRequestStatusUpdatedMessage
    {
        public int? AccountId { get; set; }
    }
}
