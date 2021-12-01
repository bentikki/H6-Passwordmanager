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
    public class UserServiceTests : ServiceTestBase
    {
        public UserServiceTests()
        {
        }


        #region CreateUserAsync
        [Fact]
        public async Task CreateUserAsync_ValidInput_ShouldReturnIUser()
        {
            // Arrange
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = "CreateUser" + _validTestUsername;
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
            string testUsername = "AuthenticateAsync_ValidCredentials" + GetTestName;

            try
            {
                // Arrange
                IUser user = this.CreateTestUser(GetTestName);
                AuthenticateResponse authenticateResponse;
                AuthenticateRequest authenticateRequest = new AuthenticateRequest();
                authenticateRequest.Username = user.Username;
                authenticateRequest.Password = _validTestPassword;

                // Act
                authenticateResponse = await this._userService.AuthenticateAsync(authenticateRequest);

                // Assert
                Assert.NotNull(authenticateResponse);
                Assert.NotEqual(0, authenticateResponse.Id);
                Assert.Equal(user.Username, authenticateResponse.Username);
                Assert.False(string.IsNullOrEmpty(authenticateResponse.TokenSet.AccessToken));
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(GetTestName);
            }
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidPassword_ShouldReturnInvalidCredentialsException()
        {
            string testUsername = "AuthenticateAsync_ValidCredentials" + GetTestName;

            try
            {
                // Arrange
                IUser user = this.CreateTestUser(testUsername);

                AuthenticateRequest authenticateRequest = new AuthenticateRequest();
                authenticateRequest.Username = _validTestUsername;
                authenticateRequest.Password = _validTestPassword + "TnvalidAddition";

                // Act & Assert
                await Assert.ThrowsAsync<InvalidCredentialsException>(() => this._userService.AuthenticateAsync(authenticateRequest));

                // Cleanup
                Assert.True(_userService.DeleteUserAsync(user).Result);
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }

        }

        [Fact]
        public async Task AuthenticateAsync_InvalidUsername_ShouldReturnInvalidCredentialsException()
        {
            string testUsername = "AuthenticateAsync_InvalidUsername" + GetTestName;

            try
            {
                // Arrange
                IUser user = this.CreateTestUser(testUsername);

                AuthenticateRequest authenticateRequest = new AuthenticateRequest();
                authenticateRequest.Username = "TnvalidAddition" + _validTestUsername;
                authenticateRequest.Password = _validTestPassword;

                // Act & Assert
                await Assert.ThrowsAsync<InvalidCredentialsException>(() => this._userService.AuthenticateAsync(authenticateRequest));

                // Cleanup
                Assert.True(_userService.DeleteUserAsync(user).Result);

            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }

        }

        #endregion AuthenticateAsync

        #region DeleteUserAsync

        [Fact]
        public async Task DeleteUserAsync_ExistingUser_ShouldReturnTrue()
        {
            // Arrange
            bool deletedSuccess = false;

            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = "DeleteUser" + _validTestUsername;
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
            createUserRequest.Username = "invalidtest" + _validTestUsername;
            createUserRequest.Password = "invalidtest" + _validTestPassword;
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
            string testUsername = "GetUserByIdAsync_ValidId" + GetTestName;

            try
            {
                // Arrange
                IUser user = this.CreateTestUser(testUsername);
                int validUserId;
                IUser fetchedUserObject = null;

                validUserId = user.Id;

                // Act
                fetchedUserObject = await this._userService.GetUserByIdAsync(validUserId);

                // Assert
                Assert.NotNull(fetchedUserObject);
                Assert.NotEqual(0, fetchedUserObject.Id);

                await this._userService.DeleteUserAsync(user);
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }

        }

        [Fact]
        public async Task GetUserByIdAsync_NonExistingId_ShouldReturnNull()
        {
            string testUsername = "GetUserByIdAsync_NonExistingId" + GetTestName;

            try
            {
                // Arrange
                IUser user = this.CreateTestUser(testUsername);
                int validUserId;
                IUser fetchedUserObject = null;

                validUserId = user.Id;

                // Cleanup
                Assert.True(this._userService.DeleteUserAsync(user).Result);

                // Act
                fetchedUserObject = await this._userService.GetUserByIdAsync(validUserId);

                // Assert
                Assert.Null(fetchedUserObject);
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }



       
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
            string testUsername = "GetByUsernameAsync_ValidUsername" + GetTestName;

            try
            {
                // Arrange
                IUser user = this.CreateTestUser(testUsername);
                IUser fetchedUserObject = null;
                string validUsername;

                validUsername = user.Username;

                // Act
                fetchedUserObject = await this._userService.GetUserByUsernameAsync(validUsername);

                // Assert
                Assert.NotNull(fetchedUserObject);
                Assert.NotEqual(0, fetchedUserObject.Id);

                await this._userService.DeleteUserAsync(user);
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }
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
