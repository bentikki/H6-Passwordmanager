using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using PasswordManagerAPI.Authorization;
using PasswordManagerAPI.Models.Users;
using PasswordManagerAPI.Services;
using System.Threading.Tasks;
using System.Net.Mime;
using PasswordManagerAPI.Models;
using PasswordManagerAPI.CustomExceptions;
using PasswordClassLibrary.Logging;

namespace PasswordManagerAPI.Controllers
{
    /// <summary>
    /// Controller handling the logic behind the /users API endpoint.
    /// Allows for manipulation of users.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class UsersController : CustomControllerBase
    {
        private IUserService _userService;

        /// <summary>
        /// Controller handling the logic behind the /users API endpoint.
        /// Allows for manipulation of users.
        /// </summary>
        /// <param name="userService">IUserService set in startup</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Reached by POST /Users
        /// Creates a user entity based on the provided CreateUserRequest.
        /// </summary>
        /// <param name="createUserRequest">CreateUserRequest containing the data neede while creating user entities.</param>
        /// <returns>CreateUserResponse containing the created IUsee object.</returns>
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

        /// <summary>
        /// Reached by POST /Users/Authenticate
        /// Autheticates a user if the provided AuthenticateRequest contains valid login information.
        /// </summary>
        /// <param name="authenticateRequest">AuthenticateRequest containing login information.</param>
        /// <returns>AuthenticateResponse if the login information was valid.</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest authenticateRequest)
        {
            try
            {
                // Authenticate the user using the IUserService.
                AuthenticateResponse authenticateResponse = await _userService.AuthenticateAsync(authenticateRequest);

                // Set the refreshToken cookie in header response
                setTokenCookie("refreshToken", authenticateResponse.TokenSet.RefreshToken.Token);
                
                return Ok(authenticateResponse);
            }
            catch (ArgumentException e)
            {
                IncidentLogger.GetLogger.LogMessageAsync(IncidentLevel.MINOR, "An input was in the wrong format.", e);

                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
            catch(InvalidCredentialsException e){

                IncidentLogger.GetLogger.LogMessageAsync(IncidentLevel.CRITICAL, "A user tried logging in with invalid credentials.", e);

                string errorMessage = e.Message;
                return Unauthorized(new ErrorResponse(errorMessage));
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