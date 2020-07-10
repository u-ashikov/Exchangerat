namespace Exchangerat.Clients.Services.Contracts.Clients
{
    using Exchangerat.Clients.Models.Clients;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClientService
    {
        Task<Result<int>> Create(ClientInputModel model);

        Task<Result<int>> GetIdByUserId();

        Task<Result<IEnumerable<ClientOutputModel>>> GetAllByUserIds(IEnumerable<string> userIds);
    }
}
