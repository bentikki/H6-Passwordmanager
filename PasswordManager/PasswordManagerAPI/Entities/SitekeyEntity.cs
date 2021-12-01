using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;
using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.Models.Sitekeys;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Text.Json.Serialization;

namespace PasswordManagerAPI.Entities
{
    /// <summary>
    /// Database entity used to map the Sitekeys table.
    /// </summary>
    [Table("Sitekeys")]
    public class SitekeyEntity : ISitekey
    {
        public int Id { get; set; }

        public int FK_Users_Id { get; set; }

        public string Sitename { get; set; }

        public string LoginName { get; set; }

        public string LoginPassword { get; set; }
    }
}
