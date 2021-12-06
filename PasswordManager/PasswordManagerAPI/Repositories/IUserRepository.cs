using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Repositories.RepositoryBase;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories
{
    
    /// <summary>
    /// Interface contract used by user repositories.
    /// </summary>
    public interface IUserRepository
    {
        Task<bool> DeleteAsync(IUser entity);
        Task<IUser> CreateAsync(IUser entity);
        Task<IUser> GetAsync(int id);
        Task<IUser> GetByUsernameAsync(string username);
        Task<IUser> GetByTokenAsync(string refreshTokenValue);
        Task<IAuthenticateUser> GetAuthenticateUserAsync(string username);
    }
}
