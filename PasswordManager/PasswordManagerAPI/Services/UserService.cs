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
using PasswordManagerAPI.TokenHandlers.AccessTokens;
using PasswordManagerAPI.TokenHandlers.RefreshTokens;

namespace PasswordManagerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;
        private readonly IAccessTokenHandler _accessTokenHandler;
        private readonly IRefreshTokenHandler _refreshTokenHandler; 

        public UserService(
            IOptions<AppSettings> appSettings,
            IUserRepository userRepository,
            IHashingService hashingService,
            IAccessTokenHandler accessTokenHandler,
            IRefreshTokenHandler refreshTokenHandler)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _hashingService = hashingService;
            _accessTokenHandler = accessTokenHandler;
            _refreshTokenHandler = refreshTokenHandler;
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
            if (string.IsNullOrEmpty(createUserRequest.Username)) throw new ArgumentNullException(nameof(createUserRequest.Username), "Username must not be null.");
            if (string.IsNullOrEmpty(createUserRequest.Password)) throw new ArgumentNullException(nameof(createUserRequest.Password), "Password must not be null.");
            if (createUserRequest.Password.Length > 128) throw new ArgumentNullException(nameof(createUserRequest.Password), "Password must not be longer than 128 characters.");
            if (createUserRequest.Username.Length > 100) throw new ArgumentNullException(nameof(createUserRequest.Username), "Username must not be longer than 100 characters.");

            // Mail validation
            MailAddress mail;
            try
            {
                mail = new MailAddress(createUserRequest.Username);
                
            }catch(Exception e)
            {
                throw new ArgumentException(e.Message, nameof(createUserRequest.Username));
            }
            if (mail.Host != "zbc.dk") throw new ArgumentException("The mail must be @ZBC.dk domain", nameof(createUserRequest.Username));

            // Check if an user with the provided username already exists - throw an exception if it does.
            IUser existingUser = await this.GetUserByUsernameAsync(createUserRequest.Username);
            if(existingUser != null)
            {
                throw new ArgumentException("A user with the provided username allready exist.", nameof(createUserRequest.Username));
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
            if (userToBeDeleted.Id <= 0) throw new ArgumentNullException(nameof(userToBeDeleted), "The user object must have a valid id.");

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
            if (id <= 0) throw new ArgumentException(nameof(id), "A valid id must be provided.");

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
            if (string.IsNullOrEmpty(username) ||string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username), "Username must not be null.");

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
            // Get user matching the attempted login.
            IAuthenticateUser authenticateUser = await this._userRepository.GetAuthenticateUserAsync(authenticateRequest.Username);

            // Compare hashed input password, against the value contained in the database, if the comparison fails -> throw an InvalidCredentialsException exception.
            // If the user does not exist, throw an InvalidCredentialsException exception.
            if (authenticateUser == null || !this._hashingService.CompareStringToHash(authenticateRequest.Password, authenticateUser.PasswordHash))
                throw new InvalidCredentialsException("Invalid credentials.");

            // authentication successful so generate access and refresh tokens
            string accessToken = this._accessTokenHandler.GenerateToken(authenticateUser.Id.ToString());
            IRefreshToken refreshToken = this._refreshTokenHandler.GenerateRefreshToken();

            // Add newly created RefreshToken to user and delete old ones.
            bool newTokenSat = await this._userRepository.SetNewRefreshTokenForUserAsync(refreshToken, authenticateUser);

            if (!newTokenSat)
            {
                throw new ArgumentException("The new token could not be set.", nameof(authenticateRequest));
            }


            return new AuthenticateResponse((UserEntity)authenticateUser, accessToken, refreshToken.Token);
        }


        public async Task<AuthenticateResponse> RefreshAccessTokenAsync(string token)
        {
            IUser user = await this._userRepository.GetByActiveTokenAsync(token);

            if (user == null) throw new ArgumentException("Invalid token value", nameof(token));

            IRefreshToken oldRefreshToken = await this._userRepository.GetTokenByUserAsync(user);

            if (oldRefreshToken == null || oldRefreshToken.Revoked != null || oldRefreshToken.IsExpired)
            {
                throw new InvalidCredentialsException("Invalid refresh token.");
            }

            // authentication successful so generate access and refresh tokens
            string accessToken = this._accessTokenHandler.GenerateToken(user.Id.ToString());
            IRefreshToken refreshToken = this._refreshTokenHandler.GenerateRefreshToken();

            // Add newly created RefreshToken to user and delete old ones.
            bool newTokenSat = await this._userRepository.SetNewRefreshTokenForUserAsync(refreshToken, user);

            if (!newTokenSat)
            {
                throw new ArgumentException("The new token could not be set.", nameof(token));
            }

            return new AuthenticateResponse((UserEntity)user, accessToken, refreshToken.Token);
        }

        public async Task RevokeAccessTokenAsync(string token)
        {
            IUser user = await this._userRepository.GetByActiveTokenAsync(token);
            IRefreshToken oldRefreshToken = await this._userRepository.GetTokenByUserAsync(user);

            if (oldRefreshToken == null || oldRefreshToken.Revoked != null || oldRefreshToken.IsExpired)
            {
                throw new InvalidCredentialsException("Invalid refresh token.");
            }

            await this._userRepository.RevokeAccessTokenAsync(oldRefreshToken);
        }

    }
}
