namespace Exchangerat.Requests.Gateway.Models.Requests
{
    using System;

    public class RequestOutputModel
    {
        public int Id { get; set; }

        public string RequestType { get; set; }

        public string UserId { get; set; }

        public int? AccountId { get; set; }

        public int? AccountTypeId { get; set; }

        public string Status { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
