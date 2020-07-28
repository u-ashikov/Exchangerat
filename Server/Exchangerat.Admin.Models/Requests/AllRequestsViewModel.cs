namespace Exchangerat.Admin.Models.Requests
{
    using System.Collections.Generic;


    public class AllRequestsViewModel
    {
        public List<ClientRequestViewModel> Requests { get; set; } = new List<ClientRequestViewModel>();

        public int TotalItems { get; set; }
    }
}
