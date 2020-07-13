namespace Exchangerat.Admin.Services.Contracts.Identity
{
    using Exchangerat.Admin.Models.Identity;
    using Refit;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        [Post("/api/Identity/Login")]
        Task<UserOutputModel> Login([Body]LoginFormModel model, [Query]bool adminLogin);
    }
}
