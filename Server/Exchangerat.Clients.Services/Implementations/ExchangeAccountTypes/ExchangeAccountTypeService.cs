namespace Exchangerat.Clients.Services.Implementations.ExchangeAccountTypes
{
    using Data;
    using Exchangerat.Clients.Data.Models;
    using Exchangerat.Clients.Models.ExchangeAccountTypes;
    using Exchangerat.Clients.Services.Contracts.ExchangeAccountTypes;
    using Exchangerat.Services.Common;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExchangeAccountTypeService : DataService<ExchangeAccountType>, IExchangeAccountTypeService
    {
        public ExchangeAccountTypeService(ClientsDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<ExchangeAccountTypeOutputModel>> GetAll()
            => await this.All()
                .Select(at => new ExchangeAccountTypeOutputModel()
                {
                    Id = at.Id,
                    Name = at.Name,
                    Description = at.Description
                })
                .AsNoTracking()
                .ToListAsync();
    }
}
