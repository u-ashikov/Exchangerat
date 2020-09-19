namespace Exchangerat.IntegrationTests.Common.Extensions
{
    using Helpers;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public static class WebHostBuilderExtensions
    {
        public static HttpClient CreateClientWithJwtAuthentication<T>(this WebApplicationFactory<T> factory, string[] roles = null)
            where T : class
        {
            var client = factory.CreateClient();

            var token = JwtTokenGeneratorService.GenerateJwtToken(roles);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}
