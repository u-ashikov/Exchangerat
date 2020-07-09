namespace Exchangerat.Requests.Gateway
{
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Middlewares;
    using Services.Identity;

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
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .UseWebService(env)
                .UseJwtHeaderAuthentication();
        }
    }
}
