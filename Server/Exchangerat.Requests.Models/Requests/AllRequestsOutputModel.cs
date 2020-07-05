namespace Exchangerat.Requests.Models.Requests
{
    using System.Collections.Generic;

    public class AllRequestsOutputModel
    {
        public List<RequestOutputModel> Requests { get; set; } = new List<RequestOutputModel>();
    }
}
