namespace Exchangerat.Clients.Services.Contracts.Transactions
{
    using Exchangerat.Clients.Models.Transactions;
    using Infrastructure;
    using System.Threading.Tasks;

    public interface ITransactionService
    {
        Task<Result> Create(TransactionInputModel model, string userId);
    }
}
