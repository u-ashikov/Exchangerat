namespace Exchangerat.Requests
{
    using Constants;
    using Data;
    using Exchangerat.Infrastructure;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using Exchangerat.Requests.Services.Implementations.Requests;
    using Exchangerat.Services.Identity;
    using MassTransit;
    using Messages;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Middlewares;
    using Refit;
    using Services.Contracts.ExchangeAccounts;
    using Services.Contracts.Identity;
    using Services.Contracts.RequestTypes;
    using Services.Implementations.RequestTypes;
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
                .AddDataStore<RequestsDbContext>(this.Configuration)
                .AddAuthenticationWithJwtBearer(this.Configuration)
                .AddHttpContextAccessor()
                .AddTransient<JwtHeaderAuthenticationMiddleware>()
                .AddControllers();

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
                .AddMassTransit(mt =>
                {
                    mt.AddConsumer<RequestApprovedConsumer>();
                    mt.AddConsumer<RequestCancelledConsumer>();

                    mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rmq =>
                    {
                        rmq.Host("rabbitmq://localhost");

                        rmq.ReceiveEndpoint(nameof(RequestApprovedConsumer), endPoint =>
                        {
                            endPoint.ConfigureConsumer<RequestApprovedConsumer>(bus);
                        });

                        rmq.ReceiveEndpoint(nameof(RequestCancelledConsumer), endPoint =>
                        {
                            endPoint.ConfigureConsumer<RequestCancelledConsumer>(bus);
                        });
                    }));
                })
                .AddMassTransitHostedService();

            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRequestTypeService, RequestTypeService>();
            services.AddScoped<ICurrentTokenService, CurrentTokenService>();
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
