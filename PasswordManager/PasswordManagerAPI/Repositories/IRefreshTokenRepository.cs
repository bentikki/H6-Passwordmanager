using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<IRefreshToken> Get(string token);
        Task<bool> TokenValidAsync(string token);
        Task<IRefreshToken> GetByUserAsync(IUser user);
        Task<bool> SetNewRefreshTokenForUserAsync(IRefreshToken refreshToken, IUser user);
        Task<bool> RevokeAccessTokenAsync(IRefreshToken refreshToken);
    }
}
