using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using PasswordManagerAPI.Authorization;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Services;
using System.Threading.Tasks;
using System.Net.Mime;
using PasswordManagerAPI.Models;
using PasswordManagerAPI.Models.RefreshTokens;
using PasswordManagerAPI.CustomExceptions;
using PasswordClassLibrary.Logging;

namespace PasswordManagerAPI.Controllers
{
    /// <summary>
    /// Controller handling the logic behind the /token API endpoint.
    /// Allows for manipulation of refresh tokens.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class TokenController : CustomControllerBase
    {
        private IRefreshTokenService _refreshTokenService;
        private IUserService _userService;

        /// <summary>
        /// Controller handling the logic behind the /token API endpoint.
        /// Allows for manipulation of refresh tokens.
        /// </summary>
        /// <param name="refreshTokenService">IRefreshTokenService used to manipulate tokens.</param>
        /// <param name="userService">IUserService used for manipulating users.</param>
        public TokenController(
            IRefreshTokenService refreshTokenService,
            IUserService userService)
        {
            _refreshTokenService = refreshTokenService;
            _userService = userService;
        }

        /// <summary>
        /// Reached by POST /Token/refresh
        /// Allows for refreshing refreshTokens in the database.
        /// </summary>
        /// <returns>AuthenticateResponse contining the new token set.</returns>
        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken()
        {
            try
            {
                // Gets the token from the request cookie.
                string refreshToken = Request.Cookies["refreshToken"];

                // Get IUser object matching the token.
                IUser user = await _userService.GetUserByTokenAsync(refreshToken);
                if (user == null)
                    throw new InvalidCredentialsException("Invalid token");

                // Use the IRefreshTokenService to replace the token set 
                RefreshTokenResponse response = await _refreshTokenService.RefreshAccessTokenAsync(refreshToken, user);

                setTokenCookie("refreshToken", response.TokenSet.RefreshToken.Token);
                return Ok(response);
            }
            catch (ArgumentException e)
            {
                IncidentLogger.GetLogger.LogMessageAsync(IncidentLevel.MINOR, "An input was in the wrong format.", e);

                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
            catch (InvalidCredentialsException e)
            {
                IncidentLogger.GetLogger.LogMessageAsync(IncidentLevel.MAJOR, "A user tried refreshing an expired token.", e);

                string errorMessage = e.Message;
                return Unauthorized(new ErrorResponse(errorMessage));
            }
            catch (Exception e)
            {
                IncidentLogger.GetLogger.LogMessageAsync(IncidentLevel.CRITICAL, "An unexpected error occured while refreshing token.", e);

                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
        }

        /// <summary>
        /// Reached by POST /Token/revoke
        /// Revokes the users token set. Removing thier authentication access.
        /// </summary>
        /// <param name="revokeTokenRequest">Request contining the token to revoke.</param>
        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> RevokeToken(RevokeTokenRequest revokeTokenRequest)
        {
            try
            {
                // accept refresh token in request body or cookie
                string token = revokeTokenRequest.Token ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(token)) throw new ArgumentNullException("Token is required", nameof(revokeTokenRequest));

                await _refreshTokenService.RevokeAccessTokenAsync(token);
                return Ok();

            }
            catch (ArgumentNullException e)
            {
                IncidentLogger.GetLogger.LogMessageAsync(IncidentLevel.MAJOR, "An user tried to revoke a null value token.", e);

                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
            catch (ArgumentException e)
            {
                IncidentLogger.GetLogger.LogMessageAsync(IncidentLevel.MINOR, "An input was in the wrong format.", e);

                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
            catch (Exception e)
            {
                IncidentLogger.GetLogger.LogMessageAsync(IncidentLevel.CRITICAL, "An unexpected error occured while generating password.", e);

                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
        }




    }
}