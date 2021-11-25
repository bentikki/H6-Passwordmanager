using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.Users
{
    /// <summary>
    /// The contract interface used for User objects.
    /// </summary>
    public interface IUser
    {
        public int Id { get; }
        public string Username { get; }
        public DateTime CreatedAt { get; }
    }
}
