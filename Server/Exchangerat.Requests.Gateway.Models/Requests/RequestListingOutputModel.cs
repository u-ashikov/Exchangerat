namespace Exchangerat.Requests.Gateway.Models.Requests
{
    using System.Collections.Generic;

    public class RequestListingOutputModel
    {
        public List<ClientRequestOutputModel> Requests { get; set; } = new List<ClientRequestOutputModel>();
    }
}
