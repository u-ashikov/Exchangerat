namespace Exchangerat.Clients
{
    using Common;
    using Data;
    using Exchangerat.Clients.Services.Contracts.Clients;
    using Exchangerat.Clients.Services.Implementations.Clients;
    using Exchangerat.Infrastructure;
    using Exchangerat.Services.ExchangeAccounts;
    using Infrastructure.Extensions;
    using Messages;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Refit;
    using Services.Contracts.ExchangeAccounts;
    using Services.Contracts.ExchangeAccountTypes;
    using Services.Contracts.Funds;
    using Services.Contracts.Identity;
    using Services.Contracts.Transactions;
    using Services.Implementations.ExchangeAccounts;
    using Services.Implementations.ExchangeAccountTypes;
    using Services.Implementations.Funds;
    using Services.Implementations.Transactions;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var serviceEndpoints = this.Configuration
                .GetSection(nameof(ServiceEndpoints))
                .Get<ServiceEndpoints>(config => config.BindNonPublicProperties = true);

            services
                .AddWebService<ClientsDbContext>(this.Configuration)
                .AddTransient<IClientService, ClientService>()
                .AddTransient<IExchangeAccountService, ExchangeAccountService>()
                .AddTransient<ITransactionService, TransactionService>()
                .AddTransient<IIdentityNumberGenerator, IdentityNumberGenerator>()
                .AddTransient<IFundService, FundService>()
                .AddTransient<IExchangeAccountTypeService, ExchangeAccountTypeService>();

            services
                .AddRefitClient<IIdentityService>()
                .WithConfiguration(serviceEndpoints.Identity);

            services
                .AddMessaging(
                    typeof(AccountCreatedConsumer),
                    typeof(AccountBlockedConsumer),
                    typeof(AccountDeletedConsumer));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app.UseWebService(env)
                .Initialize()
                .SeedData();
    }
}
