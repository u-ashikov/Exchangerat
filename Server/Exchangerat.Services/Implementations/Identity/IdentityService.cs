namespace Exchangerat.Services.Implementations.Identity
{
    using Data;
    using Exchangerat.Data.Models;
    using Exchangerat.Models.Identity;
    using Exchangerat.Services.Contracts.Identity;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class IdentityService : IIdentityService
    {
        private readonly ExchangeratDbContext dbContext;

        private readonly UserManager<User> userManager;

        private readonly IJwtTokenGeneratorService jwtTokenGenerator;

        public IdentityService(ExchangeratDbContext dbContext, UserManager<User> userManager, IJwtTokenGeneratorService jwtTokenGenerator)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<IdentityResult> Register(RegisterUserInputModel model)
        {
            var user = new User()
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address
            };

            var identityResult = await this.userManager.CreateAsync(user, model.Password);

            return identityResult;
        }
    }
}
