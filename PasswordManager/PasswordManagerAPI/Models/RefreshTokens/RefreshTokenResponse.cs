using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.RefreshTokens
{
    public class RefreshTokenResponse
    {
        public IRefreshToken RefreshToken { get; }
        public string AccessToken { get; }

        public RefreshTokenResponse(IRefreshToken refreshToken, string accessToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }
    }
}
