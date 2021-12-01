using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.DependencyInjection;
using PasswordManagerAPI.Entities;
using PasswordManagerAPI.Services;
using System;
using Xunit;
using System.Threading.Tasks;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.CustomExceptions;
using PasswordManagerAPI.Models.Sitekeys;
using System.Collections.Generic;
using System.Linq;

namespace PasswordManagerAPI.Tests
{
    public class SitekeyServiceTests : ServiceTestBase
    {
        private readonly ISitekeyService _sitekeyService;

        public SitekeyServiceTests()
        {
            this._sitekeyService = _startup.GetRequiredService<ISitekeyService>();
        }

        //CreateAsync
        [Fact]
        public async Task CreateSitekeyAsync_ValidSitekey_ShouldReturnISitekey()
        {
            string testUsername = "CreateSitekeyAsync_ValidSitekey" + GetTestName;

            try
            {
                // Arrange
                IUser testUser = this.CreateTestUser(testUsername);
                CreateSitekeyRequest createSitekeyRequest = new CreateSitekeyRequest("TestSitename", "testLoginName", "testPassword");

                // Act 
                ISitekey createdSiteKey = await this._sitekeyService.CreateSitekeyAsync(createSitekeyRequest, testUser);

                // Assert
                Assert.NotNull(createdSiteKey);
                Assert.NotEqual("", createdSiteKey.LoginName);
                Assert.False((string.IsNullOrEmpty(createdSiteKey.Sitename) && string.IsNullOrWhiteSpace(createdSiteKey.Sitename)));
                Assert.False((string.IsNullOrEmpty(createdSiteKey.LoginName) && string.IsNullOrWhiteSpace(createdSiteKey.LoginName)));
                Assert.False((string.IsNullOrEmpty(createdSiteKey.LoginPassword) && string.IsNullOrWhiteSpace(createdSiteKey.LoginPassword)));
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }
        }

        [Theory]
        [InlineData(null,null,null)]
        [InlineData(null, "siteKeyName", "siteUsername")]
        [InlineData("siteKeyName", null, "siteUsername")]
        [InlineData("siteKeyName", "siteUsername", null)]
        public async void CreateSitekeyAsync_NullValues_ShouldThrowArgumentNullException(string siteKeyName, string siteUsername, string sitePassword)
        {
            string testUsername = "CreateSitekeyAsync_NullValues" + GetTestName;
            try
            {
                // Arrange
                IUser testUser = this.CreateTestUser(testUsername);
                CreateSitekeyRequest createSitekeyRequest = new CreateSitekeyRequest(siteKeyName, siteUsername, sitePassword);

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() => _sitekeyService.CreateSitekeyAsync(createSitekeyRequest, testUser));

            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }
        }

        [Fact]
        public async Task CreateSitekeyAsync_NullUser_ShouldThrowArgumentNullException()
        {
            // Arrange
            IUser testUser = null;
            CreateSitekeyRequest createSitekeyRequest = new CreateSitekeyRequest("TestSitename", "testLoginName", "testPassword");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sitekeyService.CreateSitekeyAsync(createSitekeyRequest, testUser));
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("   ", "   ", "   ")]
        public async Task CreateSitekeyAsync_InvalidValues_ShouldThrowArgumentException(string siteKeyName, string siteUsername, string sitePassword)
        {
            string testUsername = "CreateSitekeyAsync_NullValues" + GetTestName;

            try
            {
                // Arrange
                IUser testUser = this.CreateTestUser(testUsername);
                CreateSitekeyRequest createSitekeyRequest = new CreateSitekeyRequest(siteKeyName, siteUsername, sitePassword);

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => _sitekeyService.CreateSitekeyAsync(createSitekeyRequest, testUser));
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }
        }

        [Fact]
        public async Task CreateSitekeyAsync_InvalidUserID_ShouldThrowArgumentException()
        {
            string testUsername = "CreateSitekeyAsync_InvalidUserID" + GetTestName;

            try
            {
                // Arrange
                IUser testUser = this.CreateTestUser(testUsername);
                int testuserID = testuser.Id;
                UserEntity userEntity = (UserEntity)testuser;
                userEntity.Id = 0;

                CreateSitekeyRequest createSitekeyRequest = new CreateSitekeyRequest("TestSitename", "testLoginName", "testPassword");

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => _sitekeyService.CreateSitekeyAsync(createSitekeyRequest, userEntity));
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }
        }

        //Task<IEnumerable<ISitekey>> GetAllSitekeysByUserAsync(IUser user)
        [Fact]
        public async Task GetAllSitekeysByUserAsync_UserWithSitekeys_ShouldReturnIEnumerableContainingCreatedSitekeys()
        {
            string testUsername = "CreateSitekeyAsync_InvalidUserID" + GetTestName;

            try
            {
                // Arrange
                IUser testUser = this.CreateTestUser(testUsername);
                CreateSitekeyRequest createSitekeyRequest = new CreateSitekeyRequest("TestSitename", "testLoginName", "testPassword");
                CreateSitekeyRequest createSitekeyRequest1 = new CreateSitekeyRequest("TestSitename1", "testLoginName1", "testPassword1");
                CreateSitekeyRequest createSitekeyRequest2 = new CreateSitekeyRequest("TestSitename2", "testLoginName2", "testPassword2");
                ISitekey testSitekey = await _sitekeyService.CreateSitekeyAsync(createSitekeyRequest, testUser);
                ISitekey testSitekey1 = await _sitekeyService.CreateSitekeyAsync(createSitekeyRequest1, testUser);
                ISitekey testSitekey2 = await _sitekeyService.CreateSitekeyAsync(createSitekeyRequest2, testUser);

                // Act
                IEnumerable<ISitekey> usersSitekeys = await this._sitekeyService.GetAllSitekeysByUserAsync(testUser);

                // Assert
                Assert.NotNull(usersSitekeys);
                Assert.Contains(usersSitekeys, x => x.Sitename == createSitekeyRequest.Sitename);
                Assert.Contains(usersSitekeys, x => x.LoginName == createSitekeyRequest.LoginName);
                Assert.Contains(usersSitekeys, x => x.LoginPassword == createSitekeyRequest.LoginPassword);
                Assert.Contains(usersSitekeys, x => x.Sitename == createSitekeyRequest1.Sitename);
                Assert.Contains(usersSitekeys, x => x.LoginName == createSitekeyRequest1.LoginName);
                Assert.Contains(usersSitekeys, x => x.LoginPassword == createSitekeyRequest1.LoginPassword);
                Assert.Contains(usersSitekeys, x => x.Sitename == createSitekeyRequest2.Sitename);
                Assert.Contains(usersSitekeys, x => x.LoginName == createSitekeyRequest2.LoginName);
                Assert.Contains(usersSitekeys, x => x.LoginPassword == createSitekeyRequest2.LoginPassword);
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }
        }

        [Fact]
        public async Task GetAllSitekeysByUserAsync_UserWithoutAnySitekeys_ShouldReturnEmptyIEnumerable()
        {
            string testUsername = "CreateSitekeyAsync_InvalidUserID" + GetTestName;

            try
            {
                // Arrange
                IUser testUser = this.CreateTestUser(testUsername);

                // Act
                IEnumerable<ISitekey> usersSitekeys = await this._sitekeyService.GetAllSitekeysByUserAsync(testUser);

                // Assert
                Assert.Empty(usersSitekeys);
            }
            finally
            {
                // Cleanup
                this.DeleteTestUser(testUsername);
            }
        }


    }
}
