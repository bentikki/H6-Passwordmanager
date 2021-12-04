using PasswordClassLibrary.Validation;
using PasswordManagerAPI.Models.Sitekeys;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Services
{
    public class SitekeyService : ISitekeyService
    {
        private readonly ISitekeyRepository _sitekeyRepository;

        public SitekeyService(ISitekeyRepository sitekeyRepository)
        {
            _sitekeyRepository = sitekeyRepository;
        }

        /// <summary>
        /// Creates the sitekey from provided request.
        /// Throws ArgumentNullException: if som of the provided arguments are null.
        /// Throws ArgumentException: if the given arguments does not live up to the sites requirements.
        /// Throws RepositoryNotAvailableException: if an error occurs in the repository.
        /// </summary>
        /// <param name="createSitekeyRequest">The sitekey to create</param>
        /// <param name="user">The owner of the sitekey to create</param>
        /// <returns>ISitekey object from the newly created entity</returns>
        public async Task<ISitekey> CreateSitekeyAsync(CreateSitekeyRequest createSitekeyRequest, IUser user)
        {
            // Validate input
            if (user == null) throw new ArgumentNullException(nameof(user), "The provided user must not be null.");
            Validator.ValidateAndThrow("UserID", user.Id);
            this.ValidateSitekey(createSitekeyRequest);

            // Create the sitekey via the repo.
            ISitekey createdSitekey = await this._sitekeyRepository.CreateAsync(createSitekeyRequest, user);

            return createdSitekey;
        }

        public async Task<IEnumerable<ISitekey>> GetAllSitekeysByUserAsync(IUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "The provided user must not be null.");
            Validator.ValidateAndThrow("UserID", user.Id);

            // Get the list of sitekeys belonging to user.
            IEnumerable<ISitekey> usersSitekeys = await this._sitekeyRepository.GetAllByUserAsync(user);

            return usersSitekeys;
        }


        private void ValidateSitekey(CreateSitekeyRequest createSitekeyRequest)
        {
            Validator.ValidateAndThrow("Sitekey-Sitename", createSitekeyRequest.Sitename);
            Validator.ValidateAndThrow("Sitekey-LoginName", createSitekeyRequest.LoginName);
            Validator.ValidateAndThrow("Sitekey-LoginPassword", createSitekeyRequest.LoginPassword);
        }
    }
}
