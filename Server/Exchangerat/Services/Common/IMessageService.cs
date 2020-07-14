namespace Exchangerat.Services.Common
{
    using Data.Models;
    using System.Threading.Tasks;

    public interface IMessageService : IDataService<Message>
    {
        Task SaveMessage(Message message);
    }
}
