using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Repositories.RepositoryBase;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories
{
    public interface IUserRepository
    {
        Task<bool> DeleteAsync(IUser entity);
        Task<IUser> CreateAsync(IUser entity);
        Task<IUser> GetAsync(int id);
        Task<IUser> GetByUsernameAsync(string username);
        Task<IUser> GetByActiveTokenAsync(string refreshTokenValue);
        Task<IAuthenticateUser> GetAuthenticateUserAsync(string username);

        Task<IRefreshToken> GetTokenByUserAsync(IUser user);
        Task<bool> SetNewRefreshTokenForUserAsync(IRefreshToken refreshToken, IUser user);
        Task<bool> RevokeAccessTokenAsync(IRefreshToken refreshToken);
    }
}
