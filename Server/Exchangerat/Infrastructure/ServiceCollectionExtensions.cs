namespace Exchangerat.Infrastructure
{
    using GreenPipes;
    using Hangfire;
    using MassTransit;
    using Messages;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Services.Identity;
    using System;
    using System.Text;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationSettings(this IServiceCollection services,
            IConfiguration configuration)
            => services
                .Configure<AppSettings>(configuration
                    .GetSection(nameof(AppSettings)));

        public static IServiceCollection AddAuthenticationWithJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration
                .GetSection(nameof(AppSettings))
                .GetValue<string>(nameof(AppSettings.Secret));

            var key = Encoding.ASCII.GetBytes(secret);

            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        public static IServiceCollection AddDataStore<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
            => services
                .AddScoped<DbContext, TDbContext>()
                .AddDbContext<TDbContext>(options => options
                    .UseSqlServer(configuration.GetDefaultConnectionString(),
                        sqlServerOptions =>
                        {
                            sqlServerOptions.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        }));

        public static IServiceCollection AddWebService<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            services
                .AddApplicationSettings(configuration)
                .AddDataStore<TDbContext>(configuration)
                .AddAuthenticationWithJwtBearer(configuration)
                .AddHttpContextAccessor()
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddHealth(configuration)
                .AddControllers();

            return services;
        }

        public static IServiceCollection AddMessaging(
            this IServiceCollection services,
            params Type[] consumers)
        {
            services
                .AddMassTransit(mt =>
                {
                    consumers.ForEach(consumer => mt.AddConsumer(consumer));

                    mt.AddBus(context =>
                    {
                        var bus = Bus.Factory.CreateUsingRabbitMq(rmq =>
                        {
                            rmq.Host("rabbitmq", host =>
                            {
                                host.Username("rabbitmq");
                                host.Password("rabbitmq");
                            });

                            rmq.UseHealthCheck(context);

                            consumers.ForEach(consumer => rmq.ReceiveEndpoint(consumer.FullName, endpoint =>
                            {
                                endpoint.PrefetchCount = 6;
                                endpoint.UseMessageRetry(retry => retry.Interval(10, 1000));

                                endpoint.ConfigureConsumer(context, consumer);
                            }));
                        });

                        return bus;
                    });
                })
                .AddMassTransitHostedService();

            return services;
        }

        public static IServiceCollection AddHangFire(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetDefaultConnectionString()));

            services.AddHangfireServer();

            services.AddSingleton<IHostedService, MessagesHostedService>();

            return services;
        }

        public static IServiceCollection AddHealth(
            this IServiceCollection services, 
            IConfiguration configuration,
            bool includeSqlServer = true,
            bool includeMessaging = true)
        {
            var healthChecks = services.AddHealthChecks();

            if (includeSqlServer)
            {
                healthChecks
                    .AddSqlServer(configuration.GetDefaultConnectionString());
            }

            if (includeMessaging)
            {
                healthChecks
                    .AddRabbitMQ(rabbitConnectionString: "amqp://");
            }

            return services;
        }
    }
}