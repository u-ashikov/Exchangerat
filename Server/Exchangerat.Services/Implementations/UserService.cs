using Microsoft.Extensions.Options;

namespace Exchangerat.Services.Implementations
{
    using Common.Helpers;
    using Contracts;
    using Data;
    using Exchangerat.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly ExchangeratDbContext dbContext;

        private readonly UserManager<User> userManager;

        private readonly IOptions<AppSettings> appSettings;

        public UserService(ExchangeratDbContext dbContext, UserManager<User> userManager, IOptions<AppSettings> appSettings)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.appSettings = appSettings;
        }


        public IEnumerable<User> GetAll() => this.dbContext.Users.ToList();

        public async Task<UserModel> Authenticate(string username, string password)
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

            var token = this.GenerateJwtToken(user);

            return new UserModel(user, token);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.appSettings.Value.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
