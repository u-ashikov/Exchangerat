namespace Exchangerat.Clients.IntegrationTests.Controllers
{
    using Exchangerat.IntegrationTests.Common.Extensions;
    using Exchangerat.Services.Identity;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Mocks;
    using Models.Funds;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Xunit;

    public class FundsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public FundsControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Post_AddWithoutAuthenticatedUserShouldReturnUnauthorized()
        {
            var client = this.factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/Funds/Add", new TestFundInputModel(), new JsonSerializerOptions() {IgnoreNullValues = true});

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(GetInvalidFundInputModels))]
        public async Task Post_AddWithInvalidDataShouldReturnBadRequest(TestFundInputModel model)
        {
            var client = this.factory.CreateClientWithJwtAuthentication();

            var response = await client.PostAsJsonAsync("/api/Funds/Add", model, new JsonSerializerOptions() { IgnoreNullValues = true });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_AddWithNonExistingUserShouldReturnBadRequest()
        {
            var model = this.GetValidFundInputModel();

            var client = this.factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                        {
                            services.AddSingleton<ICurrentUserService>(CurrentUserServiceMock.New.Object);
                        });
                })
                .CreateClientWithJwtAuthentication();

            var response = await client.PostAsJsonAsync("/api/Funds/Add", model, new JsonSerializerOptions() { IgnoreNullValues = true });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errors = await response.Content.ReadFromJsonAsync<List<string>>();

            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e == "Sorry, you are not a client of the platform!");
        }

        [Fact]
        public async Task Post_AddWithMissingUserAccountShouldReturnBadRequest()
        {
            var model = this.GetValidFundInputModel();

            model.AccountId = 100;

            var client = this.factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<ICurrentUserService>(CurrentUserServiceMock.ExistingUser.Object);
                    });
                })
                .CreateClientWithJwtAuthentication();

            var response = await client.PostAsJsonAsync("/api/Funds/Add", model, new JsonSerializerOptions() { IgnoreNullValues = true });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errors = await response.Content.ReadFromJsonAsync<List<string>>();

            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e == "Exchange account not found.");
        }

        [Fact]
        public async Task Post_AddWithInactiveUserAccountShouldReturnBadRequest()
        {
            var model = this.GetValidFundInputModel();

            model.AccountId = 2;

            var client = this.factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<ICurrentUserService>(CurrentUserServiceMock.ExistingUser.Object);
                    });
                })
                .CreateClientWithJwtAuthentication();

            var response = await client.PostAsJsonAsync("/api/Funds/Add", model, new JsonSerializerOptions() { IgnoreNullValues = true });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errors = await response.Content.ReadFromJsonAsync<List<string>>();

            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e == "The receiver account is inactive.");
        }

        [Fact]
        public async Task Post_AddShouldWorkCorrectly()
        {
            var model = this.GetValidFundInputModel();

            var client = this.factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<ICurrentUserService>(CurrentUserServiceMock.ExistingUser.Object);
                    });
                })
                .CreateClientWithJwtAuthentication();

            var response = await client.PostAsJsonAsync("/api/Funds/Add", model, new JsonSerializerOptions() { IgnoreNullValues = true });

            response.EnsureSuccessStatusCode();

            var userFunds = await client.GetFromJsonAsync<List<TestFundOutputModel>>("/api/Funds/GetMy");

            Assert.NotEmpty(userFunds);

            var fund = userFunds.LastOrDefault();

            Assert.Equal(model.AccountId, fund.AccountId);
            Assert.Equal(model.Amount, fund.Amount);
            Assert.Equal("123456789101", fund.AccountIdentityNumber);
        }

        [Fact]
        public async Task Get_GetMyWithNonExistingClientShouldReturnBadRequest()
        {
            var client = this.factory.CreateClientWithJwtAuthentication();

            var response = await client.GetAsync("/api/Funds/GetMy");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errors = await response.Content.ReadFromJsonAsync<List<string>>();

            Assert.Single(errors);
            Assert.Contains(errors, e => e == "Sorry, you are not a client of the platform!");
        }

        [Fact]
        public async Task Get_GetMyShouldWorkCorrectly()
        {
            var client = this.factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                        {
                            services.AddSingleton<ICurrentUserService>(CurrentUserServiceMock.ExistingUser.Object);
                        });
                }).CreateClientWithJwtAuthentication();

            var response = await client.GetAsync("/api/Funds/GetMy");

            response.EnsureSuccessStatusCode();

            var funds = await response.Content.ReadFromJsonAsync<List<TestFundOutputModel>>();

            Assert.NotEmpty(funds);
        }

        public static IEnumerable<object[]> GetInvalidFundInputModels()
            => new List<object[]>()
            {
                new []
                {
                    new TestFundInputModel()
                },
                new []
                {
                    new TestFundInputModel()
                    {
                        Amount = 1,
                        AccountId = 1,
                        CardIdentityNumber = "1234"
                    }
                },
                new []
                {
                    new TestFundInputModel()
                    {
                        Amount = 1,
                        AccountId = 1,
                        CardIdentityNumber = "12345678910111"
                    }
                },
                new []
                {
                    new TestFundInputModel()
                    {
                        Amount = 0,
                        AccountId = 1,
                        CardIdentityNumber = "123456789101"
                    }
                },
                new []
                {
                    new TestFundInputModel()
                    {
                        Amount = 1001,
                        AccountId = 1,
                        CardIdentityNumber = "123456789101"
                    }
                },
                new []
                {
                    new TestFundInputModel()
                    {
                        Amount = -1,
                        AccountId = 1,
                        CardIdentityNumber = "123456789101"
                    }
                }
            };

        private TestFundInputModel GetValidFundInputModel()
            => new TestFundInputModel()
            {
                Amount = 1,
                CardIdentityNumber = "123456789101",
                AccountId = 1
            };
    }
}
