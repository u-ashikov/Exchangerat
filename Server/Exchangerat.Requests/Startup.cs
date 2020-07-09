namespace Exchangerat.Requests
{
    using Data;
    using Exchangerat.Infrastructure;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using Exchangerat.Requests.Services.Implementations.Requests;
    using Exchangerat.Services.Identity;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Refit;
    using Services.Contracts.ExchangeAccounts;
    using Services.Contracts.Identity;
    using Services.Contracts.RequestTypes;
    using Services.Implementations.RequestTypes;
    using System;

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
            services.AddWebService<RequestsDbContext>(this.Configuration);

            services
                .AddRefitClient<IIdentityService>()
                .ConfigureHttpClient(builder =>
                {
                    builder.BaseAddress = new Uri("http://localhost:5001/api/Identity");
                });

            services
                .AddRefitClient<IExchangeAccountService>()
                .ConfigureHttpClient((serviceProvider, builder) =>
                {
                    builder.BaseAddress = new Uri("http://localhost:5000/api");

                    var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

                    var currentTokenService = httpContextAccessor.HttpContext.RequestServices.GetService<ICurrentTokenService>();

                    builder.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentTokenService.Get()}");
                });

            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRequestTypeService, RequestTypeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
              .UseWebService(env)
              .Initialize()
              .SeedData();
        }
    }
}
