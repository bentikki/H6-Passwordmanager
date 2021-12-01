using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.RefreshTokens
{
    public class RefreshTokenResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public AccessRefreshTokenSet TokenSet { get; }

        public RefreshTokenResponse(AccessRefreshTokenSet tokenSet, IUser user)
        {
            Id = user.Id;
            Username = user.Username;
            TokenSet = tokenSet;
        }
    }
}
