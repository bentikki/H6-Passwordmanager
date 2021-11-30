using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.TokenHandlers.RefreshTokens.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.RefreshTokens
{
    /// <summary>
    /// Handler used to generate refresh tokens.
    /// </summary>
    public class RefreshTokenHandler : IRefreshTokenHandler
    {
        private readonly double _ttlInDays;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        /// <summary>
        /// Handler used to generate refresh tokens.
        /// </summary>
        /// <param name="refreshTokenGenerator">Refreshtoken generator to be used.</param>
        /// <param name="ttlInDays">RefreshTokens time to live in days.</param>
        public RefreshTokenHandler(IRefreshTokenGenerator refreshTokenGenerator, double ttlInDays)
        {
            _refreshTokenGenerator = refreshTokenGenerator;
            _ttlInDays = ttlInDays;
        }

        /// <summary>
        /// Generate a refresh token from the used IRefreshTokenGenerator.
        /// </summary>
        /// <returns>IRefreshToken object.</returns>
        public IRefreshToken GenerateRefreshToken()
        {
            return this._refreshTokenGenerator.GenerateRefreshToken(_ttlInDays);
        }
    }
}
