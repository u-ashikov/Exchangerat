using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Exchangerat.Requests.Gateway
{
    using Common;
    using Exchangerat.Requests.Gateway.Services.Requests;
    using Exchangerat.Services.Identity;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Middlewares;
    using Refit;
    using Services.Clients;
    using Services.ExchangeAccounts;

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
            var serviceEndpoints = this.Configuration
                .GetSection(nameof(ServiceEndpoints))
                .Get<ServiceEndpoints>(config => config.BindNonPublicProperties = true);

            services
                .AddApplicationSettings(this.Configuration)
                .AddAuthenticationWithJwtBearer(this.Configuration)
                .AddHealth(this.Configuration, includeSqlServer: false, includeMessaging: false)
                .AddHttpContextAccessor()
                .AddTransient<JwtHeaderAuthenticationMiddleware>()
                .AddScoped<ICurrentTokenService, CurrentTokenService>()
                .AddControllers();

            services
                .AddRefitClient<IClientService>()
                .WithConfiguration(serviceEndpoints.Clients);

            services
                .AddRefitClient<IRequestService>()
                .WithConfiguration(serviceEndpoints.Requests);

            services
                .AddRefitClient<IExchangeAccountService>()
                .WithConfiguration(serviceEndpoints.Clients);
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

            app
                .UseRouting()
                .UseJwtHeaderAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions
                    {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });

                    endpoints.MapControllers();
                });
        }
    }
}
