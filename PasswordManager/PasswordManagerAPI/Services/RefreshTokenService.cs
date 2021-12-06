using PasswordClassLibrary.Validation;
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
    /// <summary>
    /// Service used to manipulate RefreshToken data.
    /// </summary>
    public class RefreshTokenService : IRefreshTokenService
    {

        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAccessTokenHandler _accessTokenHandler;
        private readonly IRefreshTokenHandler _refreshTokenHandler;

        /// <summary>
        /// Service used to manipulate RefreshToken data.
        /// </summary>
        /// <param name="refreshTokenRepository">IRefreshTokenRepository used while manipulating refreshtoken entities.</param>
        /// <param name="accessTokenHandler">IAccessTokenHandler used to generate and validate access tokens.</param>
        /// <param name="refreshTokenHandler">IRefreshTokenHandler used to generate token values used in refresh tokens.</param>
        public RefreshTokenService(
            IRefreshTokenRepository refreshTokenRepository,
            IAccessTokenHandler accessTokenHandler,
            IRefreshTokenHandler refreshTokenHandler)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenHandler = accessTokenHandler;
            _refreshTokenHandler = refreshTokenHandler;
        }

        /// <summary>
        /// Refresh the provided token.
        /// </summary>
        /// <param name="token">The token to refresh.</param>
        /// <returns>RefreshTokenResponse containing the new token set</returns>
        public async Task<RefreshTokenResponse> RefreshAccessTokenAsync(string token, IUser tokenOwner)
        {
            // Validate input
            Validator.ValidateAndThrow("Token", token);

            IRefreshToken oldRefreshToken = await this._refreshTokenRepository.Get(token);

            if (oldRefreshToken == null || oldRefreshToken.Revoked != null || oldRefreshToken.IsExpired)
            {
                throw new InvalidCredentialsException("Invalid refresh token.");
            }

            // Generate a new token set of Refresh + Access tokens
            AccessRefreshTokenSet accessRefreshTokenSet = this.GenerateNewTokenSet(tokenOwner.Id.ToString());

            // Add newly created RefreshToken to user and delete old ones.
            bool newTokenSat = await this._refreshTokenRepository.SetNewRefreshTokenForUserAsync(accessRefreshTokenSet.RefreshToken, tokenOwner);

            if (!newTokenSat)
            {
                throw new ArgumentException("The new token could not be set.", nameof(token));
            }

            return new RefreshTokenResponse(accessRefreshTokenSet, tokenOwner);
        }

        /// <summary>
        /// Generates a new token set containing a RefreshToken and Access token.
        /// </summary>
        /// <param name="tokenClaimId"></param>
        /// <returns></returns>
        public AccessRefreshTokenSet GenerateNewTokenSet(string tokenClaimId)
        {
            // Validate input
            Validator.ValidateAndThrow("UserID", tokenClaimId);

            //if (tokenClaimId == null || string.IsNullOrEmpty(tokenClaimId) || string.IsNullOrWhiteSpace(tokenClaimId)) throw new ArgumentException("Invalid claim id", nameof(tokenClaimId));

            //long tokenClaimIdInt = 0;
            //bool tokenClaimIsInt = long.TryParse(tokenClaimId, out tokenClaimIdInt);
            //if(!tokenClaimIsInt || tokenClaimIdInt < 0) throw new ArgumentException("Token claim must be numeric and a valid user id.", nameof(tokenClaimId));


            // Create a new token set containing refreshtoken and access token.
            string accessToken = this._accessTokenHandler.GenerateToken(tokenClaimId);
            IRefreshToken refreshToken = this._refreshTokenHandler.GenerateRefreshToken();

            return new AccessRefreshTokenSet(refreshToken, accessToken);
        }

        /// <summary>
        /// Revokes the token entity matching the provided token value.
        /// Throws InvalidCredentialsException: If the provided token value is not valid.
        /// </summary>
        /// <param name="token">The value of the token entity to revoke.</param>
        public async Task RevokeAccessTokenAsync(string token)
        {
            // Validate input
            Validator.ValidateAndThrow("Token", token);

            IRefreshToken oldRefreshToken = await this._refreshTokenRepository.Get(token);

            if (oldRefreshToken == null || oldRefreshToken.Revoked != null || oldRefreshToken.IsExpired)
            {
                throw new InvalidCredentialsException("Invalid refresh token.");
            }

            await this._refreshTokenRepository.RevokeAccessTokenAsync(oldRefreshToken);
        }

        /// <summary>
        /// Set a the provided refresh token for the provided user.
        /// </summary>
        /// <param name="refreshToken">RefreshToken to connect to the provided user.</param>
        /// <param name="user">User to connect to the provided refreshtoken</param>
        /// <returns>True if the refreshtoken and user could be connected, false if not.</returns>
        public async Task<bool> SetNewRefreshTokenForUserAsync(IRefreshToken refreshToken, IUser user)
        {
            bool tokenSatSuccessfully = await this._refreshTokenRepository.SetNewRefreshTokenForUserAsync(refreshToken, user);
            return tokenSatSuccessfully;
        }
    }
}
