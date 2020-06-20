namespace Exchangerat.Services.Contracts.Identity
{
    using Exchangerat.Models.Identity;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task<IdentityResult> Register(RegisterUserInputModel model);
    }
}
