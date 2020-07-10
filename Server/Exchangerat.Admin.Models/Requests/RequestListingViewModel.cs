namespace Exchangerat.Admin.Models.Requests
{
    using System.Collections.Generic;

    public class RequestListingViewModel
    {
        public List<ClientRequestViewModel> Requests { get; set; } = new List<ClientRequestViewModel>();
    }
}
