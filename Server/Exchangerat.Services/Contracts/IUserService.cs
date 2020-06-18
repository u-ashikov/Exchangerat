namespace Exchangerat.Services.Contracts
{
    using Data.Models;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        IEnumerable<User> GetAll();

        Task<UserModel> Authenticate(string username, string password);
    }
}
