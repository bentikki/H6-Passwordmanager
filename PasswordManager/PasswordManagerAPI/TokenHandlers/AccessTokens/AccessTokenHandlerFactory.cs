using PasswordManagerAPI.TokenHandlers.AccessTokens.TokenGenerators;
using PasswordManagerAPI.TokenHandlers.AccessTokens.TokenValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.AccessTokens
{
    public class AccessTokenHandlerFactory
    {
        /// <summary>
        /// Returns an instance of AccessTokenHandler using JWT tokens.
        /// </summary>
        /// <param name="tokenSecret">Secret used to generate and validate access tokens.</param>
        /// <param name="tokenTTLinMinutes">The generated tokens Time to Live in minutes.</param>
        /// <returns>IAccessTokenHandler object</returns>
        public static IAccessTokenHandler GetAccessTokenHandlerJWT(string tokenSecret, double tokenTTLinMinutes)
        {
            IAccessTokenGenerator jwtGenerator = new TokenGeneratorJWT();
            IAccessTokenValidator jwtValidator = new TokenValidatorJWT();

            return new AccessTokenHandler(jwtGenerator, jwtValidator, tokenSecret, tokenTTLinMinutes);
        }
    }
}
