namespace Exchangerat.Admin.Models.Requests
{
    using System;

    public class ClientRequestViewModel
    {
        public int Id { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string RequestType { get; set; }

        public string UserId { get; set; }

        public int? AccountId { get; set; }

        public string Status { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
