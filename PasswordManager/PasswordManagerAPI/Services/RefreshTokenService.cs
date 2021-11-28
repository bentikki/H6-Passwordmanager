using PasswordManagerAPI.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUserService _userService;

        public RefreshTokenService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AuthenticateResponse> RefreshAccessTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task RevokeAccessTokenAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
