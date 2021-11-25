using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.RefreshTokens.Generators
{
    public interface IRefreshTokenGenerator
    {
        IRefreshToken GenerateRefreshToken(double ttlInDays);
    }
}
