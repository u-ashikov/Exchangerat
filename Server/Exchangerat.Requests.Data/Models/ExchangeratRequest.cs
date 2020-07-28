namespace Exchangerat.Requests.Data.Models
{
    using Enums;
    using System;

    public class ExchangeratRequest
    {
        public int Id { get; set; }

        public int RequestTypeId { get; set; }
        public virtual RequestType RequestType { get; set; }

        public int? AccountId { get; set; }

        public int? AccountTypeId { get; set; }

        public Status Status { get; set; }

        public string UserId { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
