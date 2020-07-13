namespace Exchangerat.Requests.Gateway.Services.Clients
{
    using Exchangerat.Requests.Gateway.Models.Clients;
    using Refit;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClientService
    {
        [Get("/api/Clients/GetAllByUserIds")]
        Task<IEnumerable<ClientOutputModel>> GetAllByUserIds([Query(CollectionFormat.Multi)] IEnumerable<string> userIds);
    }
}
