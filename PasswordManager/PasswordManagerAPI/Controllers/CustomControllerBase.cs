using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        /// <summary>
        /// Helper method used to set cookies value.
        /// </summary>
        /// <param name="cookieName">Name of cookie to set.</param>
        /// <param name="value">Value of cookie to set.</param>
        protected virtual void setTokenCookie(string cookieName, string value)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1),
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append(cookieName, value, cookieOptions);
        }
    }
}
