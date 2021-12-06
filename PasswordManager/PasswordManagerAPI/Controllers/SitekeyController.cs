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
using PasswordClassLibrary.Logging;

namespace PasswordManagerAPI.Controllers
{
    /// <summary>
    /// Controller handling logic behind the /Sitekeys API endpoints.
    /// Enables creation and reading of sitekey entitites.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class SitekeysController : CustomControllerBase
    {
        private readonly ISitekeyService _sitekeyService;

        /// <summary>
        /// Controller handling logic behind the /Sitekeys API endpoints.
        /// Enables creation and reading of sitekey entitites.
        /// </summary>
        /// <param name="sitekeyService">ISitekeyService set in the startup class</param>
        public SitekeysController(ISitekeyService sitekeyService)
        {
            _sitekeyService = sitekeyService;
        }


        /// <summary>
        /// Reached by POST to root "/sitekeys"
        /// Allows for creation of sitekey for the authenticated user.
        /// </summary>
        /// <param name="createSitekeyRequest">CreateSitekeyRequest object containing the information of the sitekey to be created.</param>
        /// <returns>CreateSitekeyResponse containing the created sitekey.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateSitekeyResponse>> CreateSitekey(CreateSitekeyRequest createSitekeyRequest)
        {
            try
            {
                // Get the authenticated user form context. The authenticated user is set in the JwtMiddleware class.
                IUser loggedinUser = (IUser)HttpContext.Items["User"];

                // Create the sitekey using the ISitekeyService.
                ISitekey createdSiteKey = await _sitekeyService.CreateSitekeyAsync(createSitekeyRequest, loggedinUser);

                return Ok(createdSiteKey);
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
        /// Reached by GET to root "/sitekeys"
        /// Returns a list of all the authenticated users sitekeys.
        /// </summary>
        /// <returns>List of sitekeys connected to the authenticated user.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ISitekey>>> GetAllSiteKeysFromUser()
        {
            try
            {
                // Get the authenticated user form context. The authenticated user is set in the JwtMiddleware class.
                IUser loggedinUser = (IUser)HttpContext.Items["User"];

                // Returns a list of sitekeys using the ISitekeyService, with the authenticated user as argument.
                IEnumerable usersSitekeys = await this._sitekeyService.GetAllSitekeysByUserAsync(loggedinUser);

                return Ok(usersSitekeys);
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