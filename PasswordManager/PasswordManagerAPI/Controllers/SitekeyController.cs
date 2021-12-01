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
using PasswordManagerAPI.Models.Sitekeys;
using System.Collections;
using System.Collections.Generic;

namespace PasswordManagerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class SitekeysController : CustomControllerBase
    {
        private readonly ISitekeyService _sitekeyService;
        private readonly IUserService _userService;

        public SitekeysController(ISitekeyService sitekeyService, IUserService userService)
        {
            _sitekeyService = sitekeyService;
            _userService = userService;
        }


        // Reached by POST to root "/sitekeys"
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateSitekeyResponse>> CreateSitekey(CreateSitekeyRequest createSitekeyRequest)
        {
            try
            {
                IUser loggedinUser = (IUser)HttpContext.Items["User"];
                ISitekey createdSiteKey = await _sitekeyService.CreateSitekeyAsync(createSitekeyRequest, loggedinUser);

                return Ok(createdSiteKey);
            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
        }


        // Reached by GET to root "/sitekeys"
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ISitekey>>> GetAllSiteKeysFromUser()
        {
            try
            {
                IUser loggedinUser = (IUser)HttpContext.Items["User"];
                IEnumerable usersSitekeys = await this._sitekeyService.GetAllSitekeysByUserAsync(loggedinUser);

                return Ok(usersSitekeys);
            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
        }

    }
}