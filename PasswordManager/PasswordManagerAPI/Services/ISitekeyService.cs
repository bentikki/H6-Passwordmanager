using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.Models.Sitekeys;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Services
{
    public interface ISitekeyService
    {
        Task<ISitekey> CreateSitekeyAsync(CreateSitekeyRequest createSitekeyRequest, IUser user);
        Task<IEnumerable<ISitekey>> GetAllSitekeysByUserAsync(IUser user);
    }
}
