using DBContext.Connect;
using MediaStudioService;
using MediaStudioService.AccountService;
using MediaStudioService.Core;
using MediaStudioService.Minio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MediaStudio.Core
{
    public static class ServiceProviderExtensions
    {
        public static void AddContextDb(this IServiceCollection services, IConfiguration Configuration)
        {
            string connectionString = Configuration.GetConnectionString("PostgreSQL");
            services.AddDbContext<MediaStudioContext>(options => options.UseNpgsql(connectionString));
        }

        public static void AddCustomAuthorization(this IServiceCollection services) 
        {
            // добавляем политику для метода SignUpWithRole
            services.AddAuthorization(options =>  PolicyManager.BuldAuthOption(Policy.SignUpWithRole, options));
        }

        public static void AddAuthenticationUser(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<AccountService>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => JWTManager.BuldJWTOption(Configuration, x));
        }

        public static void AddAudioControllerService(this IServiceCollection services)
        {
            services.AddTransient<AlbumService>();
            services.AddTransient<TrackService>();
        }

        public static void AddCloudService(this IServiceCollection services)
        {
            services.AddTransient<CloudServiceManager>();
            services.AddTransient<CloudService>();
        }
    }
}
