using Dapper;
using PasswordManagerAPI.Contexts;
using PasswordManagerAPI.CustomExceptions;
using PasswordManagerAPI.Entities;
using PasswordManagerAPI.Models.Sitekeys;
using PasswordManagerAPI.Models.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Repositories
{
    public class SitekeyRepositoryDB : ISitekeyRepository
    {
        private IContext _context;

        /// <summary>
        /// Repository used for dealing with Sitekey entities.
        /// </summary>
        /// <param name="context">The IContext to use while handling with database connections.</param>
        public SitekeyRepositoryDB(IContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates the Sitekey entity in the connected database.
        /// </summary>
        /// <param name="sitekey">Sitekey object to create</param>
        /// <param name="user">User object of the sitekey owner</param>
        /// <returns>ISitekey object of the created entity</returns>
        public async Task<ISitekey> CreateAsync(ISitekey sitekey, IUser user)
        {
            try
            {
                // Cast object to hold entity.
                ISitekey createdSitekey = null;

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("SitekeyCreator"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new entity, and returns the newly created entity.
                    createdSitekey = await conn.QuerySingleOrDefaultAsync<SitekeyEntity>("[SP_CreateSitekey]",
                        new
                        {
                            @UserID = user.Id,
                            @Sitename = sitekey.Sitename,
                            @LoginName = sitekey.LoginName,
                            @LoginPassword = sitekey.LoginPassword,
                        }, commandType: CommandType.StoredProcedure);
                }

                return createdSitekey;
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
        /// Returns all sitekeys connected to the provided IUser.
        /// Returns empty list if the user does not own any sitekey.
        /// Throws RepositoryNotAvailableException: If an error occurs while fetching sitekey data.
        /// </summary>
        /// <param name="user">IUser object of the sitekey owner.</param>
        /// <returns>IEnumerable of ISitekey connected to the provided IUser</returns>
        public async Task<IEnumerable<ISitekey>> GetAllByUserAsync(IUser user)
        {
            try
            {
                // Cast object to hold entity.
                IEnumerable<ISitekey> usersSitekeys = new List<ISitekey>();

                // Connect to database using connectionstring defined in appsettings.json
                using (var conn = _context.CreateConnection("SitekeyReader"))
                {
                    // Open the connection - this closes automatically in the end of the using statement. 
                    conn.Open();

                    // Call Stored Procedure on the database - this creates a new entity, and returns the newly created entity.
                    usersSitekeys = await conn.QueryAsync<SitekeyEntity>("[SP_GetAllSitekeysFromUser]",
                        new
                        {
                            @UserID = user.Id
                        }, commandType: CommandType.StoredProcedure);
                }

                return usersSitekeys;
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
