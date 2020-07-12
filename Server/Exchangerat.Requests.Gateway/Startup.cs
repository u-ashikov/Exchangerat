using Exchangerat.Requests.Gateway.Services.ExchangeAccounts;

namespace Exchangerat.Requests.Gateway
{
    using Constants;
    using Exchangerat.Requests.Gateway.Services.Requests;
    using Exchangerat.Services.Identity;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Middlewares;
    using Refit;
    using Services.Clients;
    using System;
    using System.Net.Http.Headers;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddApplicationSettings(this.Configuration)
                .AddAuthenticationWithJwtBearer(this.Configuration)
                .AddHttpContextAccessor()
                .AddTransient<JwtHeaderAuthenticationMiddleware>()
                .AddScoped<ICurrentTokenService, CurrentTokenService>()
                .AddControllers();

            services
                .AddRefitClient<IClientService>()
                .ConfigureHttpClient((serviceProvider, builder) =>
                {
                    builder.BaseAddress = new Uri("http://localhost:5000/api");

                    var requestServices = serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.RequestServices;

                    var currentToken = requestServices?.GetService<ICurrentTokenService>()?.Get();

                    if (currentToken == null)
                    {
                        return;
                    }

                    var authorizationHeader = new AuthenticationHeaderValue(WebConstants.AuthorizationScheme, currentToken);

                    builder.DefaultRequestHeaders.Authorization = authorizationHeader;
                });

            services
                .AddRefitClient<IRequestService>()
                .ConfigureHttpClient((serviceProvider, builder) =>
                {
                    builder.BaseAddress = new Uri("http://localhost:5002/api");

                    var requestServices = serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.RequestServices;

                    var currentToken = requestServices?.GetService<ICurrentTokenService>()?.Get();

                    if (currentToken == null)
                    {
                        return;
                    }

                    var authorizationHeader = new AuthenticationHeaderValue(WebConstants.AuthorizationScheme, currentToken);

                    builder.DefaultRequestHeaders.Authorization = authorizationHeader;
                });

            services
                .AddRefitClient<IExchangeAccountService>()
                .ConfigureHttpClient((serviceProvider, builder) =>
                {
                    builder.BaseAddress = new Uri("http://localhost:5000/api");

                    var requestServices = serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.RequestServices;

                    var currentToken = requestServices?.GetService<ICurrentTokenService>()?.Get();

                    if (currentToken == null)
                    {
                        return;
                    }

                    var authorizationHeader = new AuthenticationHeaderValue(WebConstants.AuthorizationScheme, currentToken);

                    builder.DefaultRequestHeaders.Authorization = authorizationHeader;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseRouting();

            app.UseJwtHeaderAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
