using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResponse> RefreshAccessTokenAsync(string token);
        Task RevokeAccessTokenAsync(string token);
    }
}
