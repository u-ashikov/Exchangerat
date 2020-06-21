namespace Exchangerat.Services.Implementations.Identity
{
    using Data;
    using Exchangerat.Data.Models;
    using Exchangerat.Models.Identity;
    using Exchangerat.Services.Contracts.Identity;
    using Exchangerat.Services.Models.Common;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class IdentityService : IIdentityService
    {
        private readonly ExchangeratDbContext dbContext;

        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;

        private readonly IJwtTokenGeneratorService jwtTokenGenerator;

        public IdentityService(ExchangeratDbContext dbContext, UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenGeneratorService jwtTokenGenerator)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<UserOutputModel>> Login(LoginUserInputModel model)
        {
            var existingUser = await this.userManager.FindByNameAsync(model.UserName);

            if (existingUser == null)
            {
                return Result<UserOutputModel>.Failure(new List<string>() { "Incorrect username or password." });
            }

            var signInResult = await this.signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return Result<UserOutputModel>.Failure(new List<string>() { "Incorrect username or password." });
            }

            var token = this.jwtTokenGenerator.GenerateJwtToken(existingUser);

            return Result<UserOutputModel>.SuccessWith(new UserOutputModel(existingUser, token));
        }

        public async Task<Result<User>> Register(RegisterUserInputModel model)
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

            var errors = identityResult.Errors.Select(e => e.Description);

            return identityResult.Succeeded
                ? Result<User>.SuccessWith(user)
                : Result<User>.Failure(errors);
        }
    }
}
