using Microsoft.Extensions.Options;
using System;
using PasswordManagerAPI.Entities;
using PasswordManagerAPI.Helpers;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Repositories;
using System.Threading.Tasks;
using System.Net.Mail;
using PasswordClassLibrary.Hashing;
using PasswordManagerAPI.CustomExceptions;
using PasswordManagerAPI.Models.RefreshTokens;
using PasswordClassLibrary.Validation;

namespace PasswordManagerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IHashingService _hashingService;


        public UserService(
            IUserRepository userRepository,
            IRefreshTokenService refreshTokenService,
            IHashingService hashingService)
        {
            _userRepository = userRepository;
            _refreshTokenService = refreshTokenService;
            _hashingService = hashingService;
        }

        /// <summary>
        /// Method to create a new user entity from CreateUserRequest.
        /// Throws ArgumentNullException: If one of the provided model values are NULL.
        /// Throws ArgumentException: If the model values does not live up to format requirements.
        /// Throes RepositoryNotAvailableException: If an error occurs at the repository level.
        /// Throws HashingIncompleteException: If an error occures during hashing.
        /// </summary>
        /// <param name="createUserRequest">Object containing the raw user information used to create entity.</param>
        /// <returns>Created IUser object.</returns>
        public async Task<IUser> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            // Field Validation 
            Validator.ValidateAndThrow("Mail", createUserRequest.Username);
            Validator.ValidateAndThrow("Password", createUserRequest.Password);

            // Check if an user with the provided username already exists - throw an exception if it does.
            IUser existingUser = await this.GetUserByUsernameAsync(createUserRequest.Username);
            if(existingUser != null)
            {
                throw new ArgumentException($"A user with the provided username {createUserRequest.Username} already exist.", nameof(createUserRequest.Username));
            }

            // Create passwordhash using IHashingService
            string hashedUserPassword = this._hashingService.GenerateHashedString(createUserRequest.Password);
            createUserRequest.Password = hashedUserPassword;

            // Create the user object in the repository.
            IUser createdUser = await this._userRepository.CreateAsync(createUserRequest);

            return createdUser;

        }

        /// <summary>
        /// Method used to delete an existing user.
        /// </summary>
        /// <param name="userToBeDeleted"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserAsync(IUser userToBeDeleted)
        {
            // Field Validation 
            if (userToBeDeleted == null) throw new ArgumentNullException(nameof(userToBeDeleted), "The user object must not be null.");
            Validator.ValidateAndThrow("UserID", userToBeDeleted.Id);

            bool deletionSuccess = false;
            
            // Delete the user using repository.
            deletionSuccess = await this._userRepository.DeleteAsync(userToBeDeleted);

            return deletionSuccess;

        }

        /// <summary>
        /// Returns IUser object matching the provided id.
        /// If the user does not exist, return null.
        /// Throws ArgumentException: If the provided id is not above 0.
        /// </summary>
        /// <param name="id">Id of the requested User entity.</param>
        /// <returns>IUser object matching the provided id, else null.</returns>
        public async Task<IUser> GetUserByIdAsync(int id)
        {
            // Input validation
            Validator.ValidateAndThrow("UserID", id);

            IUser user = null;

            // Get the requested user entity from repository.
            user = await this._userRepository.GetAsync(id);

            return user;
        }

        /// <summary>
        /// Returns IUser object matching the provided username.
        /// If the user does not exist, return null.
        /// Throws ArgumentException: If the provided username does not live up to requirement.
        /// </summary>
        /// <param name="username">Username of the requested User entity.</param>
        /// <returns>IUser object matching the provided username, else null.</returns>
        public async Task<IUser> GetUserByUsernameAsync(string username)
        {
            // Input validation
            Validator.ValidateAndThrow("Mail", username);
           
            // Mail validation
            MailAddress mail;
            try
            {
                mail = new MailAddress(username);

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, nameof(username));
            }
            if (mail.Host != "zbc.dk") throw new ArgumentException("The mail must be @ZBC.dk domain", nameof(username));


            IUser user = null;

            // Get the requested user entity from repository.
            user = await this._userRepository.GetByUsernameAsync(username);

            return user;
        }

        /// <summary>
        /// Method used to authenticate users requesting access to authorized endpoints.
        /// </summary>
        /// <param name="authenticateRequest">Authenticate information.</param>
        /// <returns>AuthenticateResponse containing authentification information.</returns>
        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest)
        {
            // Input validation
            Validator.ValidateAndThrow("Mail", authenticateRequest.Username);
            Validator.ValidateAndThrow("Password", authenticateRequest.Password);

            // Get user matching the attempted login.
            IAuthenticateUser authenticateUser = await this._userRepository.GetAuthenticateUserAsync(authenticateRequest.Username);

            // Compare hashed input password, against the value contained in the database, if the comparison fails -> throw an InvalidCredentialsException exception.
            // If the user does not exist, throw an InvalidCredentialsException exception.
            if (authenticateUser == null || !this._hashingService.CompareStringToHash(authenticateRequest.Password, authenticateUser.PasswordHash))
                throw new InvalidCredentialsException("Invalid credentials.");

            // authentication successful so generate access and refresh tokens
            AccessRefreshTokenSet accessRefreshTokenSet = _refreshTokenService.GenerateNewTokenSet(authenticateUser.Id.ToString());

            // Add newly created RefreshToken to user and delete old ones.
            bool newTokenSat = await this._refreshTokenService.SetNewRefreshTokenForUserAsync(accessRefreshTokenSet.RefreshToken, authenticateUser);

            if (!newTokenSat)
            {
                throw new ArgumentException("The new token could not be set.", nameof(authenticateRequest));
            }


            return new AuthenticateResponse((UserEntity)authenticateUser, accessRefreshTokenSet);
        }

        /// <summary>
        /// Returns IUser object matching the provided token.
        /// If the token does not exist or match any user, return null.
        /// </summary>
        /// <param name="token">The token to find a matching user entity</param>
        /// <returns>IUser object matching the provided token</returns>
        public async Task<IUser> GetUserByTokenAsync(string token)
        {
            // Input validation
            Validator.ValidateAndThrow("Token", token);

            IUser user = null;

            // Get the requested user entity from repository.
            user = await this._userRepository.GetByTokenAsync(token);

            return user;
        }


    }
}
