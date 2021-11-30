using PasswordManagerAPI.Entities;
using PasswordManagerAPI.Models.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.RefreshTokens.Generators
{
    /// <summary>
    /// Used to generate refresh tokens based on RNGCryptoServiceProvider keys.
    /// </summary>
    public class RefreshTokenGeneratorRNGCrypto : IRefreshTokenGenerator
    {
        /// <summary>
        /// Generate refresh token using RNGCryptoServiceProvider to create random token.
        /// </summary>
        /// <param name="ttlInDays">The tokens Time to Live in days.</param>
        /// <returns>Newly generated refresh token.</returns>
        public IRefreshToken GenerateRefreshToken(double ttlInDays)
        {
            // generate token that is valid for the amount of days set in ttlInDays
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            IRefreshToken refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(ttlInDays),
                Created = DateTime.UtcNow
            };

            return refreshToken;
        }
    }
}
