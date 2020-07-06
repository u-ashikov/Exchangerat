namespace Exchangerat.Identity.Services.Contracts
{
    using Data.Models;
    using System.Collections.Generic;

    public interface IJwtTokenGeneratorService
    {
        string GenerateJwtToken(User user, IEnumerable<string> roles = null);
    }
}
