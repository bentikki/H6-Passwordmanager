using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.AccessTokens.TokenValidators
{
    /// <summary>
    /// Token Validator using JWT tokens.
    /// Able to return token identifier from successfull validation.
    /// </summary>
    public class TokenValidatorJWT : IAccessTokenValidator
    {
        /// <summary>
        /// Returns identifier from provided JWT token.
        /// Returns null if the validation fails.
        /// </summary>
        /// <param name="token">Token to get identifier from.</param>
        /// <param name="tokenSecret">Secret used to generate the provided token.</param>
        /// <returns>String containing the tokens identifier, in case the validation fails this will be null.</returns>
        public string GetIdFromToken(string token, string tokenSecret)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenSecret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var tokenIdValue = jwtToken.Claims.First(x => x.Type == "id").Value;

                // return token id from JWT token if validation successful
                return tokenIdValue;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
