using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.RefreshTokens
{
    public class AccessRefreshTokenSet
    {
        public IRefreshToken RefreshToken { get; }
        public string AccessToken { get; }

        public AccessRefreshTokenSet(IRefreshToken refreshToken, string accessToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }
    }
}
