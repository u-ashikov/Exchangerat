namespace Exchangerat.Clients.Services.Contracts.Clients
{
    using Exchangerat.Clients.Models.Clients;
    using Infrastructure;
    using System.Threading.Tasks;

    public interface IClientService
    {
        Task<Result> Create(ClientInputModel model);
    }
}
