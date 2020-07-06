namespace Exchangerat.Admin.Services.Contracts
{
    using Exchangerat.Admin.Models.Models.Identity;
    using Refit;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        [Post("/Login")]
        Task<UserOutputModel> Login([Body]LoginFormModel model);
    }
}
