namespace Exchangerat.Identity
{
    using Data;
    using Exchangerat.Infrastructure;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Contracts;
    using Services.Implementations;

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
            services.AddWebService<IdentityDbContext>(this.Configuration);
            services.AddUsers();

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebService(env);
            app.Initialize();
            app.SeedData();
        }
    }
}