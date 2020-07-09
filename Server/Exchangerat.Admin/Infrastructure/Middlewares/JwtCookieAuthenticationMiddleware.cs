namespace Exchangerat.Admin.Infrastructure.Middlewares
{
    using Exchangerat.Admin.Common.Constants;
    using Exchangerat.Services.Identity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class JwtCookieAuthenticationMiddleware : IMiddleware
    {
        private readonly ICurrentTokenService currentTokenService;

        public JwtCookieAuthenticationMiddleware(ICurrentTokenService currentTokenService)
        {
            this.currentTokenService = currentTokenService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Cookies[WebConstants.AuthCookieKey];

            if (!string.IsNullOrEmpty(token))
            {
                this.currentTokenService.Set(token);

                context.Request.Headers.Append(WebConstants.AuthorizationHeader, $"{WebConstants.AuthenticatioScheme} {token}");
            }

            await next.Invoke(context);
        }
    }

    public static class JwtCookieAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtCookieAuthentication(
            this IApplicationBuilder app)
            => app
                .UseMiddleware<JwtCookieAuthenticationMiddleware>()
                .UseAuthentication();
    }
}
