namespace Exchangerat.Services.Contracts.Identity
{
    using Exchangerat.Data.Models;
    using Exchangerat.Models.Identity;
    using Exchangerat.Services.Models.Common;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task<Result<User>> Register(RegisterUserInputModel model);

        Task<Result<UserOutputModel>> Login(LoginUserInputModel model);
    }
}
