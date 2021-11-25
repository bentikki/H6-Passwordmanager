using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.AccessTokens.TokenValidators
{
    /// <summary>
    /// Contract used by AccessTokenValidators.
    /// </summary>
    interface IAccessTokenValidator
    {
        string GetIdFromToken(string token, string tokenSecret);
    }
}
