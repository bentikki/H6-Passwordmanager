using Dapper;
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
    public class RefreshTokenRepositoryDB : IRefreshTokenRepository
    {
        private IContext _context;

        public RefreshTokenRepositoryDB(IContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the IRefreshToken entity matching the provided token.
        /// Returns null if no token could be found.
        /// </summary>
        /// <param name="token">Token value of the requested RefreshToken</param>
        /// <returns>IRefreshToken object matching the provided token value.</returns>
        public async Task<IRefreshToken> Get(string token)
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
                    refreshToken = await conn.QuerySingleOrDefaultAsync<RefreshTokenEntity>("[SP_GetSingleToken]",
                        new
                        {
                            @Token = token
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

        /// <summary>
        /// Returns the active IRefreshToken object from the provided IUser entity.
        /// Returns null if the provided IUser does not have any active RefreshTokens.
        /// Throws RepositoryNotAvailableException: If an error occured while fetching data from the database.
        /// </summary>
        /// <param name="user">The IUser entity owning the needed RefreshToken</param>
        /// <returns>IRefresh token entity active from the provided IUser</returns>
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

        /// <summary>
        /// Revoke the access from the provided RefreshToken.
        /// Returns true if the RefreshToken was successfully revoked.
        /// Throws RepositoryNotAvailableException: If an error occured while fetching data from the database.
        /// </summary>
        /// <param name="refreshToken">IRefreshToken that needs to be revoked.</param>
        /// <returns>Bool true if the RefreshToken could be revoked - False if not.</returns>
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


        /// <summary>
        /// Used to set a new  RefreshToken for a user.
        /// Returns a boolean with the result.
        /// Throws RepositoryNotAvailableException: If an error occured while fetching data from the database.
        /// </summary>
        /// <param name="refreshToken">RefreshToken to set as active for user.</param>
        /// <param name="user">User to add the RefreshToken to.</param>
        /// <returns>Bool true if the refreshtoken could be set succesfully - False if not</returns>
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

        public async Task<bool> TokenValidAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
