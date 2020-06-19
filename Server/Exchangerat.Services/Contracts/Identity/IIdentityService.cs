namespace Exchangerat.Services.Contracts.Identity
{
    using Data.Models;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        IEnumerable<User> GetAll();

        Task<UserServiceModel> Authenticate(string username, string password);
    }
}
