﻿namespace Exchangerat.Identity.Services.Implementations
{
    using Contracts;
    using Data.Enums;
    using Data.Models;
    using Exchangerat.Identity.Models.Identity;
    using Infrastructure;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static Exchangerat.Identity.Common.Constants.WebConstants;

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;

        private readonly IJwtTokenGeneratorService jwtTokenGenerator;

        public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenGeneratorService jwtTokenGenerator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<UserOutputModel>> Login(LoginUserInputModel model, bool adminLogin = false)
        {
            var existingUser = await this.userManager.FindByNameAsync(model.UserName);

            if (existingUser == null)
            {
                return Result<UserOutputModel>.Failure(new List<string>() { Messages.IncorrectUserNameOrPassword });
            }

            var signInResult = await this.signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return Result<UserOutputModel>.Failure(new List<string>() { Messages.IncorrectUserNameOrPassword });
            }

            var userRoles = await this.userManager.GetRolesAsync(existingUser);

            if (adminLogin && (userRoles == null || !userRoles.Contains(Role.Administrator.ToString())))
            {
                return Result<UserOutputModel>.Failure(new List<string>() { Messages.IncorrectUserNameOrPassword });
            }

            var token = this.jwtTokenGenerator.GenerateJwtToken(existingUser, userRoles);

            return Result<UserOutputModel>.SuccessWith(new UserOutputModel(existingUser, token));
        }

        public async Task<Result<User>> Register(RegisterUserInputModel model)
        {
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var identityResult = await this.userManager.CreateAsync(user, model.Password);

            var errors = identityResult.Errors.Select(e => e.Description);

            if (identityResult.Succeeded)
            {
                await this.userManager.AddToRoleAsync(user, Role.User.ToString());
            }

            return identityResult.Succeeded
                ? Result<User>.SuccessWith(user)
                : Result<User>.Failure(errors);
        }

        public async Task<IEnumerable<string>> GetRegisterUsersIds()
         => await this.userManager.Users
             .OrderBy(u => u.UserName)
             .Select(u => u.Id)
             .ToListAsync();
    }
}
