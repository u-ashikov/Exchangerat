namespace Exchangerat.Admin
{
    using Common;
    using Data;
    using Exchangerat.Infrastructure;
    using Exchangerat.Services.Common;
    using Exchangerat.Services.Identity;
    using HealthChecks.UI.Client;
    using Infrastructure.Middlewares;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Refit;
    using Services.Contracts.Identity;
    using Services.Contracts.Requests;

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
                .AddDataStore<AdminDbContext>(this.Configuration)
                .AddAuthenticationWithJwtBearer(this.Configuration)
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddTransient<JwtCookieAuthenticationMiddleware>();
            services.AddScoped<ICurrentTokenService, CurrentTokenService>();
            services.AddTransient<IMessageService, MessageService>();

            services
                .AddRefitClient<IIdentityService>()
                .WithConfiguration(serviceEndpoints.Identity);

            services
                .AddRefitClient<IRequestService>()
                .WithConfiguration(serviceEndpoints.Requests);

            services
                .AddMessaging()
                .AddHangFire(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseJwtCookieAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions
                    {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });

                    endpoints.MapDefaultControllerRoute();
                });

            app.Initialize();
        }
    }
}
