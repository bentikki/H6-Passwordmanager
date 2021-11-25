using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.AccessTokens.TokenGenerators
{
    /// <summary>
    /// Used to generate tokens using JWT.
    /// </summary>
    public class TokenGeneratorJWT : IAccessTokenGenerator
    {

        /// <summary>
        /// Used to generate accesstokens using JWT claims.
        /// </summary>
        /// <param name="claimId">The identifier to be used as id in the claim.</param>
        /// <param name="tokenSecret">The secret to generate the token with.</param>
        /// <param name="tokenTtlInMinutes">The time to live of the token - in minutes.</param>
        /// <returns>Generated token as string</returns>
        public string GenerateToken(string claimId, string tokenSecret, double tokenTtlInMinutes)
        {
            // generate token that is valid for the amount of minutes in tokenTtlInMinutes.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", claimId) }),
                Expires = DateTime.UtcNow.AddMinutes(tokenTtlInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
