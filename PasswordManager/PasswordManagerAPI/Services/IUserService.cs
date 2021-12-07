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
        Task<IUser> GetUserByIdAsync(int id);
        Task<IUser> GetUserByUsernameAsync(string username);
        Task<IUser> GetUserByTokenAsync(string token);
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest);
        Task<IUser> CreateUserAsync(CreateUserRequest createUserRequest);
        Task<bool> DeleteUserAsync(IUser userToBeDeleted);
    }
}
