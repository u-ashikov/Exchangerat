namespace Exchangerat.Messages.Admin
{
    public abstract class BaseRequestStatusUpdatedMessage
    {
        public int RequestId { get; set; }

        public string UserId { get; set; }

        public string RequestType { get; set; }
    }
}
