﻿namespace Exchangerat.Admin.Models.Requests
{
    using Exchangerat.Models.Pagination;
    using System.Collections.Generic;

    public class RequestListingViewModel
    {
        public List<ClientRequestViewModel> Requests { get; set; } = new List<ClientRequestViewModel>();

        public SearchFormModel Search { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
