using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using PasswordManagerAPI.Authorization;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Services;
using System.Threading.Tasks;
using System.Net.Mime;
using PasswordManagerAPI.Models;

namespace PasswordManagerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // Reached POST: api/Users
        [AllowAnonymous]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest createUserRequest)
        {
            try
            {
                IUser createdUser = await _userService.CreateUserAsync(createUserRequest);

                // Create return object
                CreateUserResponse createUserResponse = new CreateUserResponse();
                createUserResponse.User = createdUser;

                return Ok(createUserResponse);
            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage +" - " + e.InnerException.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            try
            {
                AuthenticateResponse authenticateResponse = await _userService.AuthenticateAsync(model);
                setTokenCookie(authenticateResponse.RefreshToken);
                return Ok(authenticateResponse);
            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return Unauthorized(new ErrorResponse(errorMessage));
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken()
        {
            try
            {
                string refreshToken = Request.Cookies["refreshToken"];
                AuthenticateResponse response = await _userService.RefreshAccessTokenAsync(refreshToken);
                setTokenCookie(response.RefreshToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return Unauthorized(new ErrorResponse(errorMessage));
            }
        }

        [HttpPost("revoke-token")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RevokeToken(RevokeTokenRequest model)
        {
            try
            {
                // accept refresh token in request body or cookie
                string token = model.Token ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(token)) throw new ArgumentException("Token is required", nameof(model));

                await _userService.RevokeAccessTokenAsync(token);
                return Ok();

            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
        }


        // helper methods
        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1),
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

    }
}