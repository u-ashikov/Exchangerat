namespace Exchangerat.Requests.Services.Implementations.Requests
{
    using Exchangerat.Infrastructure;
    using Exchangerat.Requests.Data;
    using Exchangerat.Requests.Models.Requests;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class RequestService : IRequestService
    {
        private readonly RequestsDbContext dbContext;

        public RequestService(RequestsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<AllRequestsOutputModel>> GetAll()
        {
            var result = new AllRequestsOutputModel();

            var requests = await this.dbContext.ExchangeratRequests
                .Select(er => new RequestOutputModel() 
                {
                   Id = er.Id,
                   RequestType = er.RequestType.Type,
                   Status = er.Status.ToString(),
                   IssuedAt = er.IssuedAt
                })
                .AsNoTracking()
                .ToListAsync();

            result.Requests = requests;

            return Result<AllRequestsOutputModel>.SuccessWith(result);
        }
    }
}
