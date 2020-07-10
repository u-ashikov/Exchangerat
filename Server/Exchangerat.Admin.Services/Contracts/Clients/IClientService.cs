namespace Exchangerat.Admin.Services.Contracts.Clients
{
    using Exchangerat.Admin.Models.Clients;
    using Refit;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClientService
    {
        [Get("/Clients/GetAllByUserIds")]
        Task<IEnumerable<ClientViewModel>> GetAllByUserIds([Query(CollectionFormat.Multi)]IEnumerable<string> userIds);
    }
}
