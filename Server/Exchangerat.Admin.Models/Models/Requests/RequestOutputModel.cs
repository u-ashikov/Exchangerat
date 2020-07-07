namespace Exchangerat.Admin.Models.Models.Requests
{
    using System;

    public class RequestOutputModel
    {
        public int Id { get; set; }

        public string RequestType { get; set; }

        public string Status { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
