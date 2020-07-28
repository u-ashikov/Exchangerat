namespace Exchangerat.Requests.Models.Models.Requests
{
    using System.Collections.Generic;

    public class AllRequestsOutputModel
    {
        public List<RequestOutputModel> Requests { get; set; } = new List<RequestOutputModel>();

        public int TotalItems { get; set; }
    }
}
