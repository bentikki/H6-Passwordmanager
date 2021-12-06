using Dapper;
using Microsoft.Extensions.DependencyInjection;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PasswordManagerAPI.Tests
{
    public abstract class ServiceTestBase
    {
        protected static IServiceProvider _startup;
        protected readonly IUserService _userService;
        protected readonly string _validTestPassword;
        protected readonly string _validTestUsername;
        protected IUser testuser;

        public ServiceTestBase()
        {
            ServiceTestBase._startup = Program.CreateHostBuilder(new string[] { }).Build().Services;
            this._userService = _startup.GetRequiredService<IUserService>();

            this._validTestUsername = "TestUser@zbc.dk";
            this._validTestPassword = "testpasswoweqdasdgytiewradsafqwsada12e12442weqweqjgjtird";
        }


        public string GetTestName { 
            get 
            {
                long randomNumber = new Random().Next(0, 100000);
                return randomNumber + _validTestUsername;
            } 
        }

        protected IUser CreateTestUser(string methodName)
        {
            
            CreateUserRequest createUserRequest = new CreateUserRequest();
            createUserRequest.Username = methodName;
            createUserRequest.Password = _validTestPassword;

            IUser user = _userService.CreateUserAsync(createUserRequest).Result;
            
            this.testuser = user;

            return user;
        }

        protected void DeleteTestUser(string methodName)
        {

            try
            {
                string sql = "delete from Users where Username = @TestUsername";

                using (var connection = new SqlConnection("Server=172.16.57.4;Database=PasswordManagerMain;User Id=TestDeleter;Password=1Ui4B0Gm7mPUAE8C0QfY5mCoaZhxhZCcSowFR6g7AlhKyAutOvpTgG2v1m917bhAwYrAXCdVkpj21hub"))
                {
                    connection.Open();
                    connection.Query(sql, new { @TestUsername = methodName });
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        protected async Task<bool> DeleteTestUser(IUser testUser)
        {
            bool testuserDeleted = await _userService.DeleteUserAsync(testUser);

            Assert.True(testuserDeleted);
            return testuserDeleted;
        }
    }
}
