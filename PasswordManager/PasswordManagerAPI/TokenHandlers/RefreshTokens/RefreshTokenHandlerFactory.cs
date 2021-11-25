using PasswordManagerAPI.TokenHandlers.RefreshTokens.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.RefreshTokens
{
    /// <summary>
    /// Factory used to generate IRefreshTokenHandler instances.
    /// </summary>
    public static class RefreshTokenHandlerFactory
    {
        /// <summary>
        /// Returns an instance of IRefreshTokenHandler.
        /// </summary>
        /// <param name="ttlInDays">The refresh tokens Time to Live in days.</param>
        /// <returns>IRefreshTokenHandler instance</returns>
        public static IRefreshTokenHandler GetRefreshTokenHandler(double ttlInDays)
        {
            IRefreshTokenGenerator refreshTokenGenerator = new RefreshTokenGeneratorRNGCrypto();
            return new RefreshTokenHandler(refreshTokenGenerator, ttlInDays);
        }
    }
}
