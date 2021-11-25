using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.Users
{
    /// <summary>
    /// Contract used for user authentication.
    /// </summary>
    public interface IAuthenticateUser : IUser
    {
        public string PasswordHash { get; }
    }
}
