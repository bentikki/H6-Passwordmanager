using System;

namespace PasswordManagerAPI.Authorization
{
    /// <summary>
    /// This simply adds an method/class decorator, which is used to grant anonymous access to API endpoints.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    { }
}