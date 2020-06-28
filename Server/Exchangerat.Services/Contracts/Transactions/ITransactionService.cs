namespace Exchangerat.Services.Contracts.Transactions
{
    using Exchangerat.Models.Transactions;
    using Exchangerat.Services.Models.Common;
    using System.Threading.Tasks;

    public interface ITransactionService
    {
        Task<Result> Create(TransactionInputModel model);
    }
}
