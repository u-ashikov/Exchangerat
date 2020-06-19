namespace Exchangerat.Services.Contracts.Identity
{
    using Exchangerat.Data.Models;

    public interface IJwtTokenGeneratorService
    {
        string GenerateJwtToken(User user);
    }
}
