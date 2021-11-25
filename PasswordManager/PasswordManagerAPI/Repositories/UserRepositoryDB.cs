using Dapper;
using Dapper.Contrib.Extensions;
using PasswordManagerAPI.Contexts;
using PasswordManagerAPI.CustomExceptions;
using PasswordManagerAPI.Entities;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories
{
    internal class UserRepositoryDB : IUserRepository
    {
        private DapperContext _context;

        public UserRepositoryDB(DapperContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<IUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IUser> UpdateAsync(IUser entity)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates the IUser object in the database.
        /// </summary>
        /// <param name="entity">The IUser entity to be created in the database.</param>
        /// <returns>The IUser object created from the database entity.</returns>
        public async Task<IUser> CreateAsync(IUser entity)
        {
            try
            {
                // Cast entity as CreateUserRequest
                CreateUserRequest createUserRequest = entity as CreateUserRequest;
                if (createUserRequest == null) throw new ArgumentException("The provided entity does not contain password information");

                IUser createdUser = null;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("UsersCreator"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    createdUser = await conn.QuerySingleOrDefaultAsync<UserEntity>("[SP_CreateUser]", 
                        new 
                        {
                            @Username = createUserRequest.Username,
                            @Password = createUserRequest.Password
                        }, commandType: CommandType.StoredProcedure);
                }

                return createdUser;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }

        /// <summary>
        /// Deletes the IUser entity in the database.
        /// </summary>
        /// <param name="entity">IUser entity to be deleted.</param>
        /// <returns>Bool true if deletion was success, false if not.</returns>
        public async Task<bool> DeleteAsync(IUser entity)
        {
            try
            {
                bool userDeletedSuccess = false;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("UsersDeleter"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    userDeletedSuccess = await conn.ExecuteScalarAsync<bool>("[SP_DeleteUser]",
                        new
                        {
                            @Identifier = entity.Id
                        }, commandType: CommandType.StoredProcedure);
                }

                return userDeletedSuccess;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }

        /// <summary>
        /// Returns the entity matching the provided id.
        /// </summary>
        /// <param name="id">Id of requested entity.</param>
        /// <returns>IUser object matching the provided id.</returns>
        public async Task<IUser> GetAsync(int id)
        {
            try
            {
                IUser user = null;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("UsersBasicReader"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    user = await conn.QuerySingleOrDefaultAsync<UserEntity>("[SP_GetSingleUser]",
                        new
                        {
                            @Identifier = id
                        }, commandType: CommandType.StoredProcedure);
                }

                return user;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }

        /// <summary>
        /// Returns the entity matching the provided username.
        /// </summary>
        /// <param name="id">Username of requested entity.</param>
        /// <returns>IUser object matching the provided username.</returns>
        public async Task<IUser> GetByUsernameAsync(string username)
        {
            try
            {
                IUser user = null;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("UsersBasicReader"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    user = await conn.QuerySingleOrDefaultAsync<UserEntity>("[SP_GetSingleUserByUsername]",
                        new
                        {
                            @Identifier = username
                        }, commandType: CommandType.StoredProcedure);
                }

                return user;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }

        /// <summary>
        /// Returns IAuthenticateUser entity matching the provided username.
        /// </summary>
        /// <param name="username">Username of IAuthenticateUser to fetch.</param>
        /// <returns>IAuthenticateUser object matching the provided username</returns>
        public async Task<IAuthenticateUser> GetAuthenticateUserAsync(string username)
        {
            try
            {
                IAuthenticateUser user = null;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("UsersBasicReader"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    user = await conn.QuerySingleOrDefaultAsync<UserEntity>("[SP_GetSingleUserByUsername]",
                        new
                        {
                            @Identifier = username
                        }, commandType: CommandType.StoredProcedure);
                }

                return user;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }

        /// <summary>
        /// Used to set a new  RefreshToken for a user.
        /// </summary>
        /// <param name="refreshToken">RefreshToken to set as active for user.</param>
        /// <param name="user">User to add the RefreshToken to.</param>
        /// <returns>Task</returns>
        public async Task<bool> SetNewRefreshTokenForUserAsync(IRefreshToken refreshToken, IUser user)
        {
            try
            {
                bool tokenReplacementSuccess = false;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("RefreshTokenCreator"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    tokenReplacementSuccess = await conn.ExecuteScalarAsync<bool>("[SP_SetActiveRefreshTokenForUser]",
                        new
                        {
                            @UserID = user.Id,
                            @Token = refreshToken.Token,
                            @Expires = refreshToken.Expires
                        }, commandType: CommandType.StoredProcedure);
                }

                return tokenReplacementSuccess;

            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }

        public async Task<IUser> GetBytokenAsync(string refreshTokenValue)
        {
            try
            {
                IUser user = null;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("RefreshTokenReader"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    user = await conn.QuerySingleOrDefaultAsync<UserEntity>("[SP_GetSingleUserByToken]",
                        new
                        {
                            @Token = refreshTokenValue
                        }, commandType: CommandType.StoredProcedure);
                }

                return user;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }

        public async Task<IRefreshToken> GetTokenByUserAsync(IUser user)
        {
            try
            {
                IRefreshToken refreshToken = null;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("RefreshTokenReader"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    refreshToken = await conn.QuerySingleOrDefaultAsync<RefreshTokenEntity>("[SP_GetTokenByUser]",
                        new
                        {
                            @UserID = user.Id
                        }, commandType: CommandType.StoredProcedure);
                }

                return refreshToken;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }

        public async Task<IUser> GetByActiveTokenAsync(string refreshTokenValue)
        {
            try
            {
                IUser user = null;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("RefreshTokenReader"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    user = await conn.QuerySingleOrDefaultAsync<UserEntity>("[SP_GetSingleUserByToken]",
                        new
                        {
                            @Token = refreshTokenValue
                        }, commandType: CommandType.StoredProcedure);
                }

                return user;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }

        public async Task<bool> RevokeAccessTokenAsync(IRefreshToken refreshToken)
        {
            try
            {
                bool tokenReplacementSuccess = false;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("RefreshTokenRevoker"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new user, and returns the newly created user.
                    tokenReplacementSuccess = await conn.ExecuteScalarAsync<bool>("[SP_RevokeRefreshToken]",
                        new
                        {
                            @Token = refreshToken.Token
                        }, commandType: CommandType.StoredProcedure);
                }

                return tokenReplacementSuccess;

            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new RepositoryNotAvailableException("The repository could not reach its destination.", e);
            }
            catch (Exception e)
            {
                throw new RepositoryNotAvailableException("An error occured in the repository.", e);
            }
        }
    }
}
