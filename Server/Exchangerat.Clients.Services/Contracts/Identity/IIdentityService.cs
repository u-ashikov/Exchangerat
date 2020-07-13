namespace Exchangerat.Clients.Services.Contracts.Identity
{
    using Refit;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        [Get("/api/Identity/GetRegisteredUsersIds")]
        Task<ICollection<string>> GetRegisteredUserIds();
    }
}
