using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.DependencyInjection;
using PasswordManagerAPI.Entities;
using PasswordManagerAPI.Services;
using System;
using Xunit;
using System.Threading.Tasks;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.CustomExceptions;

namespace PasswordManagerAPI.Tests
{
    public class UserServiceTests
    {
        // Used to build the structure supported in the Core. This is done so the DI service can be used.
        private readonly IServiceProvider _startup;
        private readonly IUserService _userService;

        private readonly string _validTestPassword;
        private readonly string _validTestUsername;

        public UserServiceTests()
        {
            this._startup = Program.CreateHostBuilder(new string[] { }).Build().Services;
            this._userService = _startup.GetRequiredService<IUserService>();
            this._validTestUsername = "TestUserToCreateAndDelete1@zbc.dk";
            this._validTestPassword = "testpasswoweqdasdgytiewradsafqwsada12e12442weqweqjgjtird";
        }


        #region CreateUserAsync
        [Fact]
        public async Task CreateUserAsync_ValidInput_ShouldReturnIUser()
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;

            // Act
            IUser user = await _userService.CreateUserAsync(createUserRequest);

            // Assert
            Assert.NotNull(user);
            Assert.NotEqual(0, user.Id);
            Assert.False(string.IsNullOrEmpty(user.Username));

            // Cleanup
            Assert.True(_userService.DeleteUserAsync(user).Result);
        }

        [Fact]
        public async Task CreateUserAsync_NullUsername_ShouldThrowArgumentNullException()
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = null;
            createUserRequest.Password = _validTestPassword;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.CreateUserAsync(createUserRequest));
        }

        [Fact]
        public async Task CreateUserAsync_NullPassword_ShouldThrowArgumentNullException()
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.CreateUserAsync(createUserRequest));
        }

        [Fact]
        public async Task CreateUserAsync_AlreadyExistingUser_ShouldThrowArgumentException()
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;

            // Create temp user
            IUser tempUser = await _userService.CreateUserAsync(createUserRequest);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(createUserRequest));

            // Cleanup
            await _userService.DeleteUserAsync(tempUser);
        }

        [Theory]
        [InlineData("    ")]
        [InlineData("teststringwithoutAt")]
        [InlineData("test@mail.com")]
        [InlineData("test@zbc.com")]
        [InlineData("@zbc.com")]
        public async Task CreateUserAsync_IllegalUsernameFormat_ShouldThrowArgumentException(string input)
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = input;
            createUserRequest.Password = _validTestPassword;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(createUserRequest));
        }

        [Theory]
        [InlineData(" ")]
        public async Task CreateUserAsync_IllegalPasswordFormat_ShouldThrowArgumentException(string input)
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = input;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(createUserRequest));
        }

        #endregion CreateUserAsync

        #region AuthenticateAsync
        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ShouldReturnAuthenticateResponse()
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;
            IUser user = await _userService.CreateUserAsync(createUserRequest);

            AuthenticateResponse authenticateResponse;
            AuthenticateRequest authenticateRequest = new AuthenticateRequest();
            authenticateRequest.Username = _validTestUsername;
            authenticateRequest.Password = _validTestPassword;

            // Act
            authenticateResponse = await this._userService.AuthenticateAsync(authenticateRequest);

            // Cleanup
            Assert.True(_userService.DeleteUserAsync(user).Result);

            // Assert
            Assert.NotNull(authenticateResponse);
            Assert.NotEqual(0, authenticateResponse.Id);
            Assert.Equal(user.Username, authenticateResponse.Username);
            Assert.False(string.IsNullOrEmpty(authenticateResponse.JwtToken));
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidPassword_ShouldReturnInvalidCredentialsException()
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;
            IUser user = await _userService.CreateUserAsync(createUserRequest);

            AuthenticateRequest authenticateRequest = new AuthenticateRequest();
            authenticateRequest.Username = _validTestUsername;
            authenticateRequest.Password = _validTestPassword + "TnvalidAddition";

            // Act & Assert
            await Assert.ThrowsAsync<InvalidCredentialsException>(() => this._userService.AuthenticateAsync(authenticateRequest));

            // Cleanup
            Assert.True(_userService.DeleteUserAsync(user).Result);
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidUsername_ShouldReturnInvalidCredentialsException()
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;
            IUser user = await _userService.CreateUserAsync(createUserRequest);

            AuthenticateRequest authenticateRequest = new AuthenticateRequest();
            authenticateRequest.Username = "TnvalidAddition" + _validTestUsername;
            authenticateRequest.Password = _validTestPassword;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidCredentialsException>(() => this._userService.AuthenticateAsync(authenticateRequest));

            // Cleanup
            Assert.True(_userService.DeleteUserAsync(user).Result);
        }

        #endregion AuthenticateAsync

        #region DeleteUserAsync

        [Fact]
        public async Task DeleteUserAsync_ExistingUser_ShouldReturnTrue()
        {
            // Arrange
            bool deletedSuccess = false;

            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;
            IUser createdUser = await _userService.CreateUserAsync(createUserRequest);

            // Act
            deletedSuccess = await _userService.DeleteUserAsync(createdUser);

            // Assert
            Assert.True(deletedSuccess);
            Assert.NotNull(createdUser);
            Assert.NotEqual(0, createdUser.Id);
            Assert.False(string.IsNullOrEmpty(createdUser.Username));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public async Task DeleteUserAsync_InvalidUserID_ShouldThrowArgumentException(int invalidUserID)
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername + "invalidtest";
            createUserRequest.Password = _validTestPassword + "invalidtest";
            createUserRequest.Id = invalidUserID;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.DeleteUserAsync(createUserRequest));
        }


        [Fact]
        public async Task DeleteUserAsync_NonexistingUser_ShouldReturnFalse()
        {
            // Arrange
            bool deleteNonExistant = true;
            bool deleteExistant = false;

            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;
            IUser createdUser = await _userService.CreateUserAsync(createUserRequest);

            // Act
            deleteExistant = await _userService.DeleteUserAsync(createdUser);

            if (deleteExistant)
            {
                deleteNonExistant = await _userService.DeleteUserAsync(createdUser);
            }

            // Assert
            Assert.False(deleteNonExistant);
        }

        #endregion DeleteUserAsync

        #region GetUserByIdAsync

        [Fact]
        public async Task GetUserByIdAsync_ValidId_ShouldReturnIUserObject()
        {
            // Arrange
            int validUserId;
            IUser fetchedUserObject = null;
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;

            IUser createdTestUser = await this._userService.CreateUserAsync(createUserRequest);
            validUserId = createdTestUser.Id;

            // Act
            fetchedUserObject = await this._userService.GetUserByIdAsync(validUserId);

            // Assert
            Assert.NotNull(fetchedUserObject);
            Assert.NotEqual(0, fetchedUserObject.Id);

            // Cleanup
            Assert.True(this._userService.DeleteUserAsync(createdTestUser).Result);
        }

        [Fact]
        public async Task GetUserByIdAsync_NonExistingId_ShouldReturnNull()
        {
            // Arrange
            int validUserId;
            IUser fetchedUserObject = null;
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;

            IUser createdTestUser = await this._userService.CreateUserAsync(createUserRequest);
            validUserId = createdTestUser.Id;

            // Cleanup
            Assert.True(this._userService.DeleteUserAsync(createdTestUser).Result);

            // Act
            fetchedUserObject = await this._userService.GetUserByIdAsync(validUserId);

            // Assert
            Assert.Null(fetchedUserObject);           
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public async Task GetUserByIdAsync_Invalidinput_ShouldThrowArgumentException(int invalidId)
        {
            // Arrange
            int userIdtoFetch = invalidId;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetUserByIdAsync(userIdtoFetch));
        }

        #endregion GetUserByIdAsync

        #region GetByUsernameAsync

        [Fact]
        public async Task GetByUsernameAsync_ValidUsername_ShouldReturnIUserObject()
        {
            // Arrange
            string validUsername;
            IUser fetchedUserObject = null;
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = _validTestUsername;
            createUserRequest.Password = _validTestPassword;

            IUser createdTestUser = await this._userService.CreateUserAsync(createUserRequest);
            validUsername = createdTestUser.Username;

            // Act
            fetchedUserObject = await this._userService.GetUserByUsernameAsync(validUsername);

            // Assert
            Assert.NotNull(fetchedUserObject);
            Assert.NotEqual(0, fetchedUserObject.Id);

            // Cleanup
            Assert.True(this._userService.DeleteUserAsync(createdTestUser).Result);
        }

        [Fact]
        public async Task GetByUsernameAsync_InvalidUsername_ShouldReturnNull()
        {
            // Arrange
            IUser fetchedUserObject = null;

            // Act
            fetchedUserObject = await this._userService.GetUserByUsernameAsync(_validTestUsername);

            // Assert
            Assert.Null(fetchedUserObject);
        }

        [Theory]
        [InlineData(null)]
        public async Task GetByUsernameAsync_InvalidFormat_ShouldThrowArgumentNullException(string invalidinput)
        {
            // Arrange
            string usernameToFetch = invalidinput;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.GetUserByUsernameAsync(usernameToFetch));
        }

        [Theory]
        [InlineData("   ")]
        [InlineData(" ")]
        public async Task GetByUsernameAsync_InvalidFormat_ShouldThrowArgumentException(string invalidinput)
        {
            // Arrange
            string usernameToFetch = invalidinput;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetUserByUsernameAsync(usernameToFetch));
        }

        #endregion GetByUsernameAsync

        #region GetUserByTokenAsync

        [Theory]
        [InlineData(null)]
        public async Task GetUserByTokenAsync_InvalidFormat_ShouldThrowArgumentNullException(string invalidinput)
        {
            // Arrange
            string tokenToFetch = invalidinput;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.GetUserByTokenAsync(tokenToFetch));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(" ")]
        public async Task GetUserByTokenAsync_InvalidFormat_ShouldThrowArgumentException(string invalidinput)
        {
            // Arrange
            string tokenToFetch = invalidinput;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetUserByTokenAsync(tokenToFetch));
        }

        #endregion GetUserByTokenAsync

    }
}
