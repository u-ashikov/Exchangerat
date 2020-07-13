namespace Exchangerat.Infrastructure
{
    using Constants;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Identity;
    using System;

    public static class HttpClientBuilderExtensions
    {
        public static void WithConfiguration(this IHttpClientBuilder httpClientBuilder, string address)
        {
            httpClientBuilder
                .ConfigureHttpClient((serviceProvider, builder) =>
                {
                    builder.BaseAddress = new Uri(address);

                    var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

                    var currentTokenService = httpContextAccessor
                        ?.HttpContext
                        ?.RequestServices
                        ?.GetService<ICurrentTokenService>();

                    var currentToken = currentTokenService?.Get();

                    if (string.IsNullOrEmpty(currentToken))
                    {
                        return;
                    }

                    builder.DefaultRequestHeaders.Add(WebConstants.AuthorizationHeaderName, $"{WebConstants.AuthorizationScheme} {currentTokenService.Get()}");
                });
        }
    }
}
