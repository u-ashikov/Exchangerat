﻿namespace Exchangerat.Admin.Services.Contracts.Requests
{
    using Exchangerat.Admin.Models.Requests;
    using Refit;
    using System.Threading.Tasks;

    public interface IRequestService
    {
        [Get("/Requests/GetAll")]
        Task<RequestListingViewModel> GetAll();
    }
}
