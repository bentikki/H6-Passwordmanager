using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using PasswordManagerAPI.Entities;
using PasswordManagerAPI.Models.Users;

namespace PasswordManagerAPI.Authorization
{
    /// <summary>
    /// This decorator can be added to API endpoints - used to force authorization check.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// This method is run before the API endpoint, which has the [Authorization] decorator.
        /// It forces a check on the context user item, in order to make sure the caller is logged in.
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // Authorization - Check if the user item is set in HttpContext. This is set in the JwtMiddleware class.
            var user = (IUser)context.HttpContext.Items["User"];
            if (user == null)
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}