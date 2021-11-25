using System.Text.Json.Serialization;
using PasswordManagerAPI.Entities;

namespace PasswordManagerAPI.Models.Users
{
    public class AuthenticateResponse
    {
        //public UserEntity LoggedInUser { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(UserEntity user, string jwtToken, string refreshToken)
        {
            //LoggedInUser = user;
            Id = user.Id;
            FirstName = "temp";
            LastName = "temp";
            Username = user.Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
