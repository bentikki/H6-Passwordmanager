using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.Users
{
    public class CreateUserRequest : IUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
    }
}
