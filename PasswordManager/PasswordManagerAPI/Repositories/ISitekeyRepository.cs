using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.Models.Sitekeys;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories
{
    
    /// <summary>
    /// Interface contract used by Sitekey repositories.
    /// </summary>
    public interface ISitekeyRepository
    {
        //Task<ISitekey> GetAsync(int id);
        Task<ISitekey> CreateAsync(ISitekey sitekey, IUser user);
        Task<IEnumerable<ISitekey>> GetAllByUserAsync(IUser user);
        //Task<bool> DeleteAsync(ISitekey sitekey);

    }
}
