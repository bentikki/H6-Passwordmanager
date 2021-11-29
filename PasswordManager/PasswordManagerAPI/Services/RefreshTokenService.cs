using PasswordManagerAPI.CustomExceptions;
using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Repositories;
using PasswordManagerAPI.TokenHandlers.AccessTokens;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAccessTokenHandler _accessTokenHandler;
        private readonly IRefreshTokenHandler _refreshTokenHandler;

        public RefreshTokenService(
            IRefreshTokenRepository refreshTokenRepository,
            IAccessTokenHandler accessTokenHandler,
            IRefreshTokenHandler refreshTokenHandler,
            IUserService userService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenHandler = accessTokenHandler;
            _refreshTokenHandler = refreshTokenHandler;
            _userService = userService;
        }

        /// <summary>
        /// Refresh the provided token.
        /// </summary>
        /// <param name="token">The token to refresh.</param>
        /// <returns>RefreshTokenResponse containing the new token set</returns>
        public async Task<RefreshTokenResponse> RefreshAccessTokenAsync(string token)
        {
            if (token == null || string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Invalid token value", nameof(token));

            IRefreshToken oldRefreshToken = await this._refreshTokenRepository.Get(token);

            if (oldRefreshToken == null || oldRefreshToken.Revoked != null || oldRefreshToken.IsExpired)
            {
                throw new InvalidCredentialsException("Invalid refresh token.");
            }

            // Get the user owning the RefreshToken - this is done in order to set the ID of the new token.
            IUser tokenOwnerUser = await this._userService.GetUserByIdAsync(oldRefreshToken.UserID);

            // authentication successful so generate access and refresh tokens
            string accessToken = this._accessTokenHandler.GenerateToken(tokenOwnerUser.Id.ToString());
            IRefreshToken refreshToken = this._refreshTokenHandler.GenerateRefreshToken();

            // Add newly created RefreshToken to user and delete old ones.
            bool newTokenSat = await this._refreshTokenRepository.SetNewRefreshTokenForUserAsync(refreshToken, tokenOwnerUser);

            if (!newTokenSat)
            {
                throw new ArgumentException("The new token could not be set.", nameof(token));
            }

            return new RefreshTokenResponse(refreshToken, accessToken);
        }

        /// <summary>
        /// Revokes the token entity matching the provided token value.
        /// Throws InvalidCredentialsException: If the provided token value is not valid.
        /// </summary>
        /// <param name="token">The value of the token entity to revoke.</param>
        public async Task RevokeAccessTokenAsync(string token)
        {
            IRefreshToken oldRefreshToken = await this._refreshTokenRepository.Get(token);

            if (oldRefreshToken == null || oldRefreshToken.Revoked != null || oldRefreshToken.IsExpired)
            {
                throw new InvalidCredentialsException("Invalid refresh token.");
            }

            await this._refreshTokenRepository.RevokeAccessTokenAsync(oldRefreshToken);
        }
    }
}
