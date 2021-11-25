using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.TokenHandlers.RefreshTokens
{
    public interface IRefreshTokenHandler
    {
        IRefreshToken GenerateRefreshToken();
    }
}
