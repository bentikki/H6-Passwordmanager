using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories
{
    /// <summary>
    /// Interface contract used by Refreshtoken repositories.
    /// </summary>
    public interface IRefreshTokenRepository
    {
        Task<IRefreshToken> Get(string token);
        Task<IRefreshToken> GetByUserAsync(IUser user);
        Task<bool> SetNewRefreshTokenForUserAsync(IRefreshToken refreshToken, IUser user);
        Task<bool> RevokeAccessTokenAsync(IRefreshToken refreshToken);
    }
}
