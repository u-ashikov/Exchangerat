namespace Exchangerat.Requests.Data.Models
{
    using System.Collections.Generic;

    public class RequestType
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public virtual ICollection<ExchangeratRequest> Requests { get; set; } = new List<ExchangeratRequest>();
    }
}
