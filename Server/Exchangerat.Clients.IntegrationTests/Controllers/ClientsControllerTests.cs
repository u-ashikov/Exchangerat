namespace Exchangerat.Clients.IntegrationTests.Controllers
{
    using Exchangerat.IntegrationTests.Common.Extensions;
    using Exchangerat.Services.Identity;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Mocks;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Xunit;

    public class ClientsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public ClientsControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;

            factory.ClientOptions.AllowAutoRedirect = false;
        }

        [Fact]
        public async Task Post_CreateClientWithoutAuthenticatedUserShouldReturnUnauthorized()
        {
            var model = new TestClientInputModel()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                Address = "Sofia"
            };

            var client = this.factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/clients/create", model, new JsonSerializerOptions() { IgnoreNullValues = true });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(GetInvalidClientInputs))]
        public async Task Post_CreateClientWithInvalidInputShouldReturnBadRequest(TestClientInputModel model)
        {
            var client = this.factory.CreateClientWithJwtAuthentication();

            var response = await client.PostAsJsonAsync("/api/clients/create", model, new JsonSerializerOptions() { IgnoreNullValues =  true });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task Post_CreateClientShouldWorkCorrectly()
        {
            var model = new TestClientInputModel()
            {
                FirstName = "Gosho",
                LastName = "Ivanov",
                Address = "Ruse"
            };

            var client = this.factory.CreateClientWithJwtAuthentication();

            var response = await client.PostAsJsonAsync("/api/clients/create", model, new JsonSerializerOptions() {IgnoreNullValues = true});

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var result = await response.Content.ReadAsStringAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Post_GetClientIdWithoutAuthenticatedUserShouldReturnUnauthorized()
        {
            var client = this.factory.CreateClient();

            var response = await client.GetAsync("/api/Clients/GetClientId");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_GetClientIdWithNonExistingClientShouldReturnBadRequest()
        {
            var client = this.factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<ICurrentUserService>(CurrentUserServiceMock.New.Object);
                    });
                })
                .CreateClientWithJwtAuthentication();

            var response = await client.GetAsync("/api/Clients/GetClientId");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errors = await response.Content.ReadFromJsonAsync<List<string>>();

            Assert.Single(errors);
            Assert.Equal("Sorry, this user is not a client.", errors.FirstOrDefault());
        }

        [Fact]
        public async Task Get_GetClientIdShouldWorkCorrectly()
        {
            var client = this.factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<ICurrentUserService>(CurrentUserServiceMock.ExistingUser.Object);
                    });
                })
                .CreateClientWithJwtAuthentication();

            var response = await client.GetAsync("/api/Clients/GetClientId");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var clientId = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(2, clientId);
        }

        [Fact]
        public async Task Get_GetAllByUserIdsWithoutAuthenticatedUserShouldReturnUnauthorized()
        {
            var client = this.factory.CreateClient();

            var response = await client.GetAsync($"/api/Clients/GetAllByUserIds?UserIds=KiroKirov");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_GetAllByUserIdsWithoutAuthenticatedAdminShouldReturnForbidden()
        {
            var client = this.factory.CreateClientWithJwtAuthentication();

            var response = await client.GetAsync($"/api/Clients/GetAllByUserIds?UserIds=KiroKirov");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Asddsa")]
        [InlineData("KrioKriov")]
        [InlineData("")]
        public async Task Get_GetAllByUserIdsShouldReturnEmptyCollectionForMissingOrInvalidIds(string userId = null)
        {
            var client = this.factory.CreateClientWithJwtAuthentication(new string[] {"Administrator"});

            var response = await client.GetAsync($"/api/Clients/GetAllByUserIds?UserIds={userId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<TestClientOutputModel>>();

            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_GetAllByUserIdsShouldWorkCorrectlyAndReturnSingleUser()
        {
            var client = this.factory.CreateClientWithJwtAuthentication(new string[] { "Administrator" });

            var response = await client.GetAsync($"/api/Clients/GetAllByUserIds?UserIds=KiroKirov");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<TestClientOutputModel>>();

            Assert.Single(result);

            var clientOutput = result.FirstOrDefault();

            Assert.Equal(2, clientOutput.Id);
            Assert.Equal("Kiro", clientOutput.FirstName);
            Assert.Equal("Kirov", clientOutput.LastName);
        }

        [Fact]
        public async Task Get_GetAllByUserIdsShouldWorkCorrectlyAndReturnMultipleUsers()
        {
            var client = this.factory.CreateClientWithJwtAuthentication(new string[] { "Administrator" });

            var response = await client.GetAsync($"/api/Clients/GetAllByUserIds?UserIds=KiroKirov&UserIds=StamatStamatov");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<TestClientOutputModel>>();

            Assert.Equal(2, result.Count);

            var clientOutput = result.FirstOrDefault();

            Assert.Contains(result, c => c.Id == 2);
            Assert.Contains(result, c => c.FirstName == "Kiro");
            Assert.Contains(result, c => c.LastName == "Kirov");
            Assert.Contains(result, c => c.Id == 3);
            Assert.Contains(result, c => c.FirstName == "Stamat");
            Assert.Contains(result, c => c.LastName == "Stamatov");
        }

        public static IEnumerable<object[]> GetInvalidClientInputs()
            => new List<object[]>()
            {
                new []
                {
                    new TestClientInputModel()
                },
                new []
                {
                    new TestClientInputModel()
                    {
                        FirstName = "Hello world, test test test test test test test test",
                        LastName = "Hello world, test test test test test test test test",
                        Address = "Test AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest AddressTest Address"
                    }
                },
                new []
                {
                    new TestClientInputModel()
                    {
                        LastName = "a",
                        Address = "asdd"
                    }
                },
                new []
                {
                    new TestClientInputModel()
                    {
                        FirstName = "a",
                        Address = "asdd"
                    }
                },
                new []
                {
                    new TestClientInputModel()
                    {
                        FirstName = "asd",
                        LastName = "asd",
                        Address = "a"
                    }
                }
            };
    }
}
