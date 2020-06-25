namespace Exchangerat.Services.Implementations.Identity
{
    using Exchangerat.Services.Contracts.Identity;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Security.Claims;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Id => this.httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
