using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Text.Json.Serialization;

namespace PasswordManagerAPI.Entities
{
    /// <summary>
    /// Database entity used to map the RefreshToken table.
    /// </summary>
    [Table("RefreshTokens")]
    public class RefreshTokenEntity : IRefreshToken
    {
        // Key is set to indicate a custom field is used as primary key.
        [Key]
        public string Token { get; set; }
        public int FK_Users_Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public DateTime? Revoked { get; set; }

        // Write(false) is set to indicate that the field should not be used by the database mapper.
        [Write(false)]
        public bool IsExpired => DateTime.UtcNow >= Expires;
        [Write(false)]
        public bool IsRevoked => Revoked != null;
        [Write(false)]
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
