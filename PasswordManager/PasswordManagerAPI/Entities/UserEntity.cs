using Dapper.Contrib.Extensions;
using PasswordManagerAPI.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Entities
{
    /// <summary>
    /// Database entity used to map the Users table.
    /// </summary>
    [Table("Users")]
    public class UserEntity : IUser, IAuthenticateUser
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        // Write(false) is set to indicate that the field should not be used by the database mapper.
        // Computed is set to indicate that the field should not be updated by the ORM.
        [Write(false)]
        [Computed]
        public DateTime CreatedAt { get; set; }
    }
}
