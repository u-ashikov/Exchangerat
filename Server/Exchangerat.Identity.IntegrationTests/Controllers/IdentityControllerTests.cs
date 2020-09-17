using System;
using System.Collections.Generic;
using System.Linq;
using Exchangerat.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Exchangerat.Identity.IntegrationTests.Controllers
{
    using Models;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Xunit;

    public class IdentityControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        private readonly HttpClient client;

        public IdentityControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            factory.ClientOptions.AllowAutoRedirect = false;

            this.client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_LoginWithInvalidModelShouldReturnBadRequest()
        {
            var model = new TestLoginUserInputModel();

            var response = await this.client.PostAsJsonAsync("/api/Identity/Login", model,
                new JsonSerializerOptions() {IgnoreNullValues = true});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task Post_LoginWithInvalidUsernameShouldReturnBadRequest()
        {
            var model = new TestLoginUserInputModel()
            {
                UserName = "kolio",
                Password = "asddsa"
            };

            var response = await this.client.PostAsJsonAsync("/api/Identity/Login", model,
                new JsonSerializerOptions() {IgnoreNullValues = true});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Theory]
        [InlineData("admin1", "asddsa")]
        [InlineData("admin", "asddsa1")]
        public async Task Post_LoginWithWrongCredentialsShouldReturnBadRequest(string username, string password)
        {
            var model = new TestLoginUserInputModel()
            {
                UserName = username,
                Password = password
            };

            var response = await this.client.PostAsJsonAsync("/api/Identity/Login", model,
                new JsonSerializerOptions() {IgnoreNullValues = true});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task Post_LoginShouldReturnStatusOkWithUserAndToken()
        {
            var model = new TestLoginUserInputModel()
            {
                UserName = "admin",
                Password = "asddsa"
            };

            var response = await this.client.PostAsJsonAsync("/api/Identity/Login", model,
                new JsonSerializerOptions() {IgnoreNullValues = true});

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TestUserOutputModel>();

            Assert.NotNull(result);
            Assert.Equal("admin", result.Username);
            Assert.NotNull(result.Token);
        }

        [Theory]
        [MemberData(nameof(GetInvalidRegisterInputModels))]
        public async Task Post_RegisterWithInvalidModelShouldReturnBadRequest(TestRegisterInputModel model)
        {
            var response = await this.client.PostAsJsonAsync("/api/Identity/Register", model,
                new JsonSerializerOptions() {IgnoreNullValues = true});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task Post_RegisterWithExistingUserShouldReturnBadRequestWithErrors()
        {
            var model = new TestRegisterInputModel()
            {
                UserName = "admin",
                Email = "admin@admin.com",
                Password = "asddsa",
                ConfirmPassword = "asddsa"
            };

            var response = await this.client.PostAsJsonAsync("/api/Identity/Register", model,
                new JsonSerializerOptions() { IgnoreNullValues = true });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<string>>();

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e == "User name 'admin' is already taken.");
            Assert.Contains(result, e => e == "Email 'admin@admin.com' is already taken.");
        }

        [Fact]
        public async Task Post_RegisterShouldReturnStatusOkWithUserData()
        {
            var response = await this.client.PostAsJsonAsync("/api/Identity/Register",
                this.GetValidRegisterInputModel(), new JsonSerializerOptions() {IgnoreNullValues = true});

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TestUserOutputModel>();

            Assert.NotNull(result);
            Assert.Equal("stamat", result.Username);
            Assert.NotNull(result.Token);
        }

        // TODO: Create extensions for empty databasee, with database, seedded database and so on like in the Tennis project.

        [Fact]
        public async Task Get_RegisteredUserIdsShouldReturnStatusOkWithIds()
        {
            var response = await this.client.GetAsync("/api/Identity/GetRegisteredUsersIds");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<string>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);
        }

        public static IEnumerable<object[]> GetInvalidRegisterInputModels()
            => new List<object[]>()
            {
                new object[] {new TestRegisterInputModel()},
                new object[]
                {
                    new TestRegisterInputModel()
                    {
                        UserName = "aa",
                        Password = "asddsa",
                        ConfirmPassword = "asddsa",
                        Email = "asd@asd.com"
                    }
                },
                new object[]
                {
                    new TestRegisterInputModel()
                    {
                        UserName = "aaaaaaaaaaaaaaaaaaaaaaaa",
                        Password = "asddsa",
                        ConfirmPassword = "asddsa",
                        Email = "asd@asd.com"
                    }
                },
                new object[]
                {
                    new TestRegisterInputModel()
                    {
                        UserName = "aaaaa",
                        Password = "asddsa",
                        ConfirmPassword = "asddsa",
                        Email = "a@a"
                    }
                },
                new object[]
                {
                    new TestRegisterInputModel()
                    {
                        UserName = "aaaaa",
                        Password = "asddsa",
                        ConfirmPassword = "asddsa",
                        Email =
                            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@abv.bg"
                    }
                },
                new object[]
                {
                    new TestRegisterInputModel()
                    {
                        UserName = "aaaaa",
                        Password = "asddsa",
                        ConfirmPassword = "asddsa",
                        Email = "aaaa"
                    }
                },
                new object[]
                {
                    new TestRegisterInputModel()
                    {
                        UserName = "aaaaa",
                        Password = "aaa",
                        ConfirmPassword = "aaa",
                        Email = "aaa@abv.bg"
                    }
                },
                new object[]
                {
                    new TestRegisterInputModel()
                    {
                        UserName = "aaaaa",
                        Password = "aaabbbcccddd",
                        ConfirmPassword = "aaabbbcccddd",
                        Email = "aaa@abv.bg"
                    }
                },
                new object[]
                {
                    new TestRegisterInputModel()
                    {
                        UserName = "aaaaa",
                        Password = "asddsa",
                        ConfirmPassword = "asdddd",
                        Email = "aaa@abv.bg"
                    }
                },
            };

        private TestRegisterInputModel GetValidRegisterInputModel()
            => new TestRegisterInputModel()
            {
                UserName = "stamat",
                Email = "stamat@stamat.com",
                Password = "asddsa",
                ConfirmPassword = "asddsa"
            };
    }
}
