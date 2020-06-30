namespace Exchangerat.Clients
{
    using Data;
    using Exchangerat.Clients.Services.Contracts.Clients;
    using Exchangerat.Clients.Services.Implementations.Clients;
    using Exchangerat.Infrastructure;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Refit;
    using Services.Contracts.ExchangeAccounts;
    using Services.Contracts.Identity;
    using Services.Contracts.Transactions;
    using Services.Implementations.ExchangeAccounts;
    using Services.Implementations.Transactions;
    using System;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebService<ClientsDbContext>(this.Configuration);

            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IExchangeAccountService, ExchangeAccountService>();
            services.AddTransient<ITransactionService, TransactionService>();

            services
                .AddRefitClient<IIdentityService>()
                .ConfigureHttpClient(builder =>
                {
                    builder.BaseAddress = new Uri("http://localhost:5001/api/Identity");
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebService(env);
            app.Initialize();
            app.SeedData();
        }
    }
}
