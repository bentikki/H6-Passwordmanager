using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.AccessTokens.TokenGenerators
{
    /// <summary>
    /// Contract used by AccessTokenGenerators.
    /// </summary>
    interface IAccessTokenGenerator
    {
        string GenerateToken(string claimId, string tokenSecret, double tokenTtlInMinutes);
    }
}
