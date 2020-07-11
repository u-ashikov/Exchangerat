namespace Exchangerat.Clients.Services.Contracts.Clients
{
    using Exchangerat.Clients.Models.Clients;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClientService
    {
        Task<Result<int>> Create(ClientInputModel model, string userId);

        Task<Result<int>> GetIdByUserId(string userId);

        Task<Result<IEnumerable<ClientOutputModel>>> GetAllByUserIds(IEnumerable<string> userIds);
    }
}
