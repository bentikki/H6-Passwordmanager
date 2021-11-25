using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.AccessTokens
{
    public interface IAccessTokenHandler
    {
        string GenerateToken(string claimId);
        string GetIdFromToken(string token);
    }
}
