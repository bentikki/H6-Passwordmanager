using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using PasswordManagerAPI.Helpers;
using PasswordManagerAPI.Services;
using PasswordManagerAPI.TokenHandlers.AccessTokens;

namespace PasswordManagerAPI.Authorization
{
    /// <summary>
    /// This is a middleware class, which is run before each API request.
    /// It checks if the Authorization header contains a JWT token with user ID.
    /// - if it does, the IUser object is added to the context.
    /// 
    /// The middleware is set in the startup class.
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        /// <summary>
        /// This is a middleware class, which is run before each API request.
        /// It checks if the Authorization header contains a JWT token with user ID.
        /// - if it does, the IUser object is added to the context.
        /// </summary>
        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }


        public async Task Invoke(HttpContext context, IUserService userService, IAccessTokenHandler accessTokenHandler)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            string userId = accessTokenHandler.GetIdFromToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = await userService.GetUserByIdAsync(int.Parse(userId));
            }

            await _next(context);
        }
    }
}