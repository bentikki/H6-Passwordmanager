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
        Task<ISitekey> GetSitekeyByIdAsync(int id);
        Task<ISitekey> CreateSitekeyAsync(CreateSitekeyRequest createSitekeyRequest, IUser user);
        Task<bool> DeleteSitekeyAsync(ISitekey sitekey);
        Task<IEnumerable<ISitekey>> GetAllSitekeysByUserAsync(IUser user);
    }
}
