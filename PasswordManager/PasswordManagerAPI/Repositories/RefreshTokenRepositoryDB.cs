using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories
{
    public class RefreshTokenRepositoryDB : IRefreshTokenRepository
    {
        public Task<IRefreshToken> GetTokenByUserAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeAccessTokenAsync(IRefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetNewRefreshTokenForUserAsync(IRefreshToken refreshToken, IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
