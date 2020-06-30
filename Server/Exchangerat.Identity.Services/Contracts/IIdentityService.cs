namespace Exchangerat.Identity.Services.Contracts
{
    using Data.Models;
    using Infrastructure;
    using Models;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task<Result<User>> Register(UserInputModel model);

        Task<Result<UserOutputModel>> Login(UserInputModel model);
    }
}
