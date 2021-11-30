using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.RefreshTokens
{
    public class RefreshTokenResponse
    {
        public AccessRefreshTokenSet TokenSet { get; }

        public RefreshTokenResponse(AccessRefreshTokenSet tokenSet)
        {
            TokenSet = tokenSet;
        }
    }
}
