namespace Exchangerat.Identity.Services.Contracts
{
    using Data.Models;
    using Infrastructure;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task<Result<User>> Register(RegisterUserInputModel model);

        Task<Result<UserOutputModel>> Login(LoginUserInputModel model, bool adminLogin = false);

        Task<IEnumerable<string>> GetRegisterUsersIds();
    }
}
