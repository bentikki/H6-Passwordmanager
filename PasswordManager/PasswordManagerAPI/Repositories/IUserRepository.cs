using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Repositories.RepositoryBase;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories
{
    public interface IUserRepository : IRepositoryRead<IUser, int>, IRepositoryWrite<IUser> 
    {
        Task<IUser> GetByUsernameAsync(string username);
        Task<IUser> GetByActiveTokenAsync(string refreshTokenValue);
        Task<IRefreshToken> GetTokenByUserAsync(IUser user);
        Task<IAuthenticateUser> GetAuthenticateUserAsync(string username);
        Task<bool> SetNewRefreshTokenForUserAsync(IRefreshToken refreshToken, IUser user);
        Task<bool> RevokeAccessTokenAsync(IRefreshToken refreshToken);
    }
}
