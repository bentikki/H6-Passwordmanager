using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordClassLibrary.Hashing;
using PasswordClassLibrary.Validation;
using PasswordClassLibrary.Validation.ValidationRules;
using PasswordManagerAPI.Authorization;
using PasswordManagerAPI.Contexts;
using PasswordManagerAPI.Helpers;
using PasswordManagerAPI.Repositories;
using PasswordManagerAPI.Services;
using PasswordManagerAPI.TokenHandlers.AccessTokens;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System.Collections.Generic;
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
            services.AddSingleton<IContext, DapperContext>();

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

            // Set rules for the Validator.
            SetValidationRuleSets();

            // Configure repositories to be used in DI.
            services.AddScoped<IUserRepository, UserRepositoryDB>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepositoryDB>();

            // Configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        }

        // configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseRouting();

            string[] allowedOrigins = { "http://localhost:4200/", "http://zbcpasswordbackend.eu.ngrok.io/" };
            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins(allowedOrigins)
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


        /// <summary>
        /// Sets validation rules for input.
        /// </summary>
        private void SetValidationRuleSets()
        {
            Validator.AddRuleSet(
                new ValidationRuleSet("Mail", new List<IValidationRule>()
                {
                    new NoNullRule(),
                    new NoEmptyStringRule(),
                    new MaxLengthRule(100),
                    new MinLengthRule(7),
                    new ValidMailRule(),
                    new MustHaveMailDomainRule("zbc.dk")
                }
            ));

            Validator.AddRuleSet(
                new ValidationRuleSet("Password", new List<IValidationRule>()
                {
                    new NoNullRule(),
                    new NoEmptyStringRule(),
                    new MinLengthRule(40),
                    new MaxLengthRule(100)
                }
            ));

            Validator.AddRuleSet(
                new ValidationRuleSet("Token", new List<IValidationRule>()
                {
                    new NoNullRule(),
                    new NoEmptyStringRule(),
                    new MinLengthRule(20),
                }
            ));

            Validator.AddRuleSet(
                new ValidationRuleSet("UserID", new List<IValidationRule>()
                {
                    new NoNullRule(),
                    new MinValueRule(1)
                }
            ));
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