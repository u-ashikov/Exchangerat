namespace Exchangerat.Clients.Services.Implementations.Clients
{
    using Data;
    using Exchangerat.Clients.Data.Models;
    using Exchangerat.Clients.Models.Clients;
    using Exchangerat.Clients.Services.Contracts.Clients;
    using Exchangerat.Services.Identity;
    using Infrastructure;
    using System.Threading.Tasks;

    public class ClientService : IClientService
    {
        private readonly ClientsDbContext dbContext;

        private readonly ICurrentUserService currentUser;

        public ClientService(ClientsDbContext dbContext, ICurrentUserService currentUser)
        {
            this.dbContext = dbContext;
            this.currentUser = currentUser;
        }

        public async Task<Result> Create(ClientInputModel model)
        {
            var userId = this.currentUser.Id;

            var client = new Client()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                UserId = userId
            };

            this.dbContext.Clients.Add(client);

            await this.dbContext.SaveChangesAsync();

            return Result.Success;
        }
    }
}
