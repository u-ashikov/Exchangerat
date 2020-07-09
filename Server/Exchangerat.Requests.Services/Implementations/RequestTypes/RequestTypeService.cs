namespace Exchangerat.Requests.Services.Implementations.RequestTypes
{
    using Data;
    using Exchangerat.Requests.Models.Models.RequestTypes;
    using Exchangerat.Requests.Services.Contracts.RequestTypes;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RequestTypeService : IRequestTypeService
    {
        private readonly RequestsDbContext dbContext;

        public RequestTypeService(RequestsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<IEnumerable<RequestTypeBaseInfoModel>>> GetAll()
        {
            var result = await this.dbContext.RequestTypes
                .Select(rt => new RequestTypeBaseInfoModel()
                {
                    Id = rt.Id,
                    Name = rt.Type
                })
                .AsNoTracking()
                .ToListAsync();

            return Result<IEnumerable<RequestTypeBaseInfoModel>>.SuccessWith(result);
        }
    }
}
