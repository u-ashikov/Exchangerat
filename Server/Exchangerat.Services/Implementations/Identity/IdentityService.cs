namespace Exchangerat.Services.Implementations.Identity
{
    using Data;
    using Exchangerat.Data.Models;
    using Exchangerat.Services.Contracts.Identity;
    using Microsoft.AspNetCore.Identity;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
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


        public IEnumerable<User> GetAll() => this.dbContext.Users.ToList();

        public async Task<UserServiceModel> Authenticate(string username, string password)
        {
            var user = await this.userManager.FindByEmailAsync(username);

            if (user == null)
            {
                return null;
            }

            var passwordMatches = await this.userManager.CheckPasswordAsync(user, password);

            if (!passwordMatches)
            {
                return null;
            }

            var token = this.jwtTokenGenerator.GenerateJwtToken(user);

            return new UserServiceModel(user, token);
        }
    }
}
