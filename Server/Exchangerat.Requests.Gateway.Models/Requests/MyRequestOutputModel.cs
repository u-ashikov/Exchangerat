namespace Exchangerat.Requests.Gateway.Models.Requests
{
    using System;

    public class MyRequestOutputModel
    {
        public string RequestType { get; set; }

        public int? AccountId { get; set; }

        public string AccountIdentityNumber { get; set; }

        public string Status { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
