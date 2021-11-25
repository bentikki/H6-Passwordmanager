using PasswordManagerAPI.Entities;
using PasswordManagerAPI.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Services
{
    /// <summary>
    /// Facade for service handlnig user functionality and interaction. 
    /// </summary>
    public interface IUserService
    {
        //AuthenticateResponseOld Authenticate(AuthenticateRequest model, string ipAddress);
        //AuthenticateResponseOld RefreshToken(string token, string ipAddress);
        //void RevokeToken(string token, string ipAddress);
        //IEnumerable<User> GetAll();
        //User GetById(int id);



        Task<IUser> GetUserByIdAsync(int id);
        Task<IUser> GetUserByUsernameAsync(string username);
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest);
        Task<AuthenticateResponse> RefreshAccessTokenAsync(string token);
        Task RevokeAccessTokenAsync(string token);
        Task<IUser> CreateUserAsync(CreateUserRequest createUserRequest);
        Task<bool> DeleteUserAsync(IUser userToBeDeleted);
    }
}
