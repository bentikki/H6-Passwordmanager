using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordClassLibrary.Hashing;
using PasswordManagerAPI.Authorization;
using PasswordManagerAPI.Contexts;
using PasswordManagerAPI.Helpers;
using PasswordManagerAPI.Repositories;
using PasswordManagerAPI.Services;
using PasswordManagerAPI.TokenHandlers.AccessTokens;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System.Globalization;

namespace PasswordManagerAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // add services to the DI container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();



            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Context
            services.AddSingleton<DapperContext>();

            // Configure repositories to be used in DI.
            services.AddScoped<IUserRepository, UserRepositoryDB>();

            // Configure DI for logic services.
            services.AddScoped<IHashingService>(s => HashingServiceFactory.GetHashingService(this.ConvertSpacedHexToByteArray(Configuration.GetValue<string>("AppSettings:PepperStringValue"))));
            
            // Configure token generators
            services.AddScoped<IAccessTokenHandler>(s => AccessTokenHandlerFactory.GetAccessTokenHandlerJWT(
                    Configuration.GetValue<string>("AppSettings:Secret"),
                    Configuration.GetValue<double>("AppSettings:AccessTokenTTLinMinutes")
                    ));
            services.AddScoped<IRefreshTokenHandler>(s => RefreshTokenHandlerFactory.GetRefreshTokenHandler(
                    Configuration.GetValue<double>("AppSettings:RefreshTokenTTLinDays")
                    ));


            // Configure DI for application services
            services.AddScoped<IUserService, UserService>();
        }

        // configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseCookiePolicy(
                new CookiePolicyOptions
                {
                    Secure = CookieSecurePolicy.Always
                });

            // global error handler
            //app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(x => x.MapControllers());
        }

        private byte[] ConvertSpacedHexToByteArray(string hexString)
        {
            string[] hexValuesSplit = hexString.Split(' ');
            byte[] data = new byte[hexValuesSplit.Length];

            for (var index = 0; index < hexValuesSplit.Length; index++)
            {
                data[index] = byte.Parse(hexValuesSplit[index], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }
    }
}