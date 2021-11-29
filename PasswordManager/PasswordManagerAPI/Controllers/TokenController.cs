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

namespace PasswordManagerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class TokenController : CustomControllerBase
    {
        private IRefreshTokenService _refreshTokenService;

        public TokenController(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken()
        {
            try
            {
                string refreshToken = Request.Cookies["refreshToken"];
                RefreshTokenResponse response = await _refreshTokenService.RefreshAccessTokenAsync(refreshToken);
                setTokenCookie("refreshToken", response.RefreshToken.Token);
                return Ok(response);
            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return Unauthorized(new ErrorResponse(errorMessage));
            }
        }

        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RevokeToken(RevokeTokenRequest model)
        {
            try
            {
                // accept refresh token in request body or cookie
                string token = model.Token ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(token)) throw new ArgumentException("Token is required", nameof(model));

                await _refreshTokenService.RevokeAccessTokenAsync(token);
                return Ok();

            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
        }




    }
}