namespace Exchangerat.Clients.Services.Implementations.Clients
{
    using Data;
    using Exchangerat.Clients.Data.Models;
    using Exchangerat.Clients.Models.Clients;
    using Exchangerat.Clients.Services.Contracts.Clients;
    using Exchangerat.Services.Identity;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
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

        public async Task<Result<int>> Create(ClientInputModel model)
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

            return Result<int>.SuccessWith(client.Id);
        }

        public async Task<Result<int>> GetIdByUserId()
        {
            var client = await this.dbContext
                .Clients
                .FirstOrDefaultAsync(c => c.UserId == this.currentUser.Id);

            if (client == null)
            {
                return Result<int>.Failure("Sorry, this user is not a client.");
            }

            return Result<int>.SuccessWith(client.Id);
        }

        public async Task<Result<IEnumerable<ClientOutputModel>>> GetAllByUserIds(IEnumerable<string> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return Result<IEnumerable<ClientOutputModel>>.SuccessWith(Enumerable.Empty<ClientOutputModel>());
            }

            var allClients = await this.dbContext.Clients
                .AsNoTracking()
                .Where(c => userIds.Contains(c.UserId))
                .Select(c => new ClientOutputModel()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    UserId = c.UserId
                })
                .ToListAsync();

            return Result<IEnumerable<ClientOutputModel>>.SuccessWith(allClients);
        }
    }
}
