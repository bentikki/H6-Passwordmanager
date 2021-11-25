using PasswordManagerAPI.TokenHandlers.AccessTokens.TokenGenerators;
using PasswordManagerAPI.TokenHandlers.AccessTokens.TokenValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.AccessTokens
{
    /// <summary>
    /// Facade used to handle AccessToken functionality.
    /// </summary>
    internal class AccessTokenHandler : IAccessTokenHandler
    {
        private readonly string _tokenSecret;
        private readonly double _tokenTTLInMinutes;
        private readonly IAccessTokenGenerator _tokenGenerator;
        private readonly IAccessTokenValidator _tokenValidator;

        /// <summary>
        /// Must be injected with the needed IAccessTokenGenerator and IAccessTokenValidator.
        /// Takes tokenSecret which is used to generate and validate accesstokens.
        /// </summary>
        /// <param name="tokenGenerator">IAccessTokenGenerator to be used while generating tokens.</param>
        /// <param name="tokenValidator">IAccessTokenValidator to be used while validating tokens</param>
        /// <param name="tokenSecret">Token secret used to generate and validate tokens.</param>
        /// <param name="tokenTTLInMinutes">The generated tokens Time to Live in minutes.</param>
        public AccessTokenHandler(IAccessTokenGenerator tokenGenerator, IAccessTokenValidator tokenValidator, string tokenSecret, double tokenTTLInMinutes)
        {
            _tokenSecret = tokenSecret;
            _tokenTTLInMinutes = tokenTTLInMinutes;
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
        }

        /// <summary>
        /// Method to generate access tokens. 
        /// </summary>
        /// <param name="claimId">The claimId to be used in the generated token. An example could be the users ID.</param>
        /// <returns>String containing the generated token.</returns>
        public string GenerateToken(string claimId)
        {
            return this._tokenGenerator.GenerateToken(claimId, _tokenSecret, _tokenTTLInMinutes);
        }

        /// <summary>
        /// Returns the ID value sat in the provided token.
        /// Returns null in case the token could not be validated, or does not contain an ID.
        /// </summary>
        /// <param name="token">The token to fetch ID value from.</param>
        /// <returns>String containing the id value of provided token, or null if the token does not contain any.</returns>
        public string GetIdFromToken(string token)
        {
            return this._tokenValidator.GetIdFromToken(token, _tokenSecret);
        }
    }
}
