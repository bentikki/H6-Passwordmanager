using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResponse> RefreshAccessTokenAsync(string token, IUser tokenOwner);
        Task RevokeAccessTokenAsync(string token);
        AccessRefreshTokenSet GenerateNewTokenSet(string tokenClaimId);
        Task<bool> SetNewRefreshTokenForUserAsync(IRefreshToken refreshToken, IUser user);

    }
}
