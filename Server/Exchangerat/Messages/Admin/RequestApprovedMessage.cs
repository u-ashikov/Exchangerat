namespace Exchangerat.Messages.Admin
{
    public class RequestApprovedMessage
    {
        public int RequestId { get; set; }

        public string UserId { get; set; }

        public int? AccountId { get; set; }
    }
}
