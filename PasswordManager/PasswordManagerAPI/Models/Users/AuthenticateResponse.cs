using System.Text.Json.Serialization;
using PasswordManagerAPI.Entities;
using PasswordManagerAPI.Models.RefreshTokens;

namespace PasswordManagerAPI.Models.Users
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public AccessRefreshTokenSet TokenSet { get; }

        [JsonIgnore] 
        public string RefreshToken { get; set; }

        public AuthenticateResponse(UserEntity user, AccessRefreshTokenSet tokenSet)
        {
            //LoggedInUser = user;
            Id = user.Id;
            FirstName = "temp";
            LastName = "temp";
            Username = user.Username;
            TokenSet = tokenSet;
        }

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
