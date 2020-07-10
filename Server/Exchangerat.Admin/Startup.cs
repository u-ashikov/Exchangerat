namespace Exchangerat.Admin
{
    using Exchangerat.Infrastructure;
    using Exchangerat.Services.Identity;
    using Infrastructure.Middlewares;
    using MassTransit;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Refit;
    using Services.Contracts.Identity;
    using Services.Contracts.Requests;
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
            services
                .AddAuthenticationWithJwtBearer(this.Configuration)
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddTransient<JwtCookieAuthenticationMiddleware>();
            services.AddScoped<ICurrentTokenService, CurrentTokenService>();

            services
                .AddRefitClient<IIdentityService>()
                .ConfigureHttpClient(builder =>
                {
                    builder.BaseAddress = new Uri("http://localhost:5001/api/Identity");
                });

            services
                .AddRefitClient<IRequestService>()
                .ConfigureHttpClient((serviceProvider, builder) =>
                {
                    builder.BaseAddress = new Uri("http://localhost:5004/api");

                    var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

                    var currentTokenService = httpContextAccessor.HttpContext.RequestServices.GetService<ICurrentTokenService>();

                    builder.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentTokenService.Get()}");
                });

            services
                .AddMassTransit(mt =>
                {
                    mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rmq =>
                    {
                        rmq.Host("rabbitmq://localhost");
                    }));
                })
                .AddMassTransitHostedService();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseJwtCookieAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
