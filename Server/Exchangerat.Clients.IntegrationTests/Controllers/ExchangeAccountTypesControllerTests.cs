namespace Exchangerat.Clients.IntegrationTests.Controllers
{
    using Exchangerat.IntegrationTests.Common.Extensions;
    using Models;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Xunit;

    public class ExchangeAccountTypesControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public ExchangeAccountTypesControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Get_GetAllWithoutAuthorizedUserShouldReturnUnauthorized()
        {
            var client = this.factory.CreateClient();

            var response = await client.GetAsync("/api/ExchangeAccountTypes/GetAll");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_GetAllShouldWorkCorrectly()
        {
            var client = this.factory.CreateClientWithJwtAuthentication();

            var response = await client.GetAsync("/api/ExchangeAccountTypes/GetAll");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<TestExchangeAccountTypeOutputModel>>();

            Assert.Equal(2, result.Count);

            Assert.Contains(result, t => t.Id == 1);
            Assert.Contains(result, t => t.Name == "Test");
            Assert.Contains(result, t => t.Description == "Test Type");

            Assert.Contains(result, t => t.Id == 2);
            Assert.Contains(result, t => t.Name == "Another Test");
            Assert.Contains(result, t => t.Description == "Another Test Type");
        }
    }
}
