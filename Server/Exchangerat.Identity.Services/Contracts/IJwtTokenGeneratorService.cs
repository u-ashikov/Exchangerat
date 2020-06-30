namespace Exchangerat.Identity.Services.Contracts
{
    using Data.Models;

    public interface IJwtTokenGeneratorService
    {
        string GenerateJwtToken(User user);
    }
}
