namespace Exchangerat.Clients.Services.Contracts.Funds
{
    using Exchangerat.Clients.Models.Funds;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFundService
    {
        Task<Result> Add(FundInputModel model, string userId);

        Task<Result<IEnumerable<FundOutputModel>>> GetMy(string userId);
    }
}
