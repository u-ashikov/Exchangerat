namespace Exchangerat.Clients.Services.Implementations.Clients
{
    using Data;
    using Exchangerat.Clients.Data.Models;
    using Exchangerat.Clients.Models.Clients;
    using Exchangerat.Clients.Services.Contracts.Clients;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static Exchangerat.Clients.Common.Constants.WebConstants;

    public class ClientService : IClientService
    {
        private readonly ClientsDbContext dbContext;

        public ClientService(ClientsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<int>> Create(ClientInputModel model, string userId)
        {
            var clientExists = await this.dbContext
                .Clients
                .AnyAsync(c => c.UserId == userId);

            if (clientExists)
            {
                return Result<int>.Failure(Messages.UserIsNotAClient);
            }

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

        public async Task<Result<int>> GetIdByUserId(string userId)
        {
            var client = await this.dbContext
                .Clients
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                return Result<int>.Failure(Messages.UserIsNotAClient);
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
