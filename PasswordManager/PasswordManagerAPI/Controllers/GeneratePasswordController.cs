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
using PasswordManagerAPI.Models.GeneratePassword;
using System.Security.Cryptography;
using System.Linq;
using PasswordClassLibrary.RandomStringGenerating;
using PasswordClassLibrary.Logging;

namespace PasswordManagerAPI.Controllers
{
    /// <summary>
    /// This controller handles the logic for the GeneratePassword API endpoint.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class GeneratePasswordController : CustomControllerBase
    {
        private readonly IRandomStringGenerator _randomStringGenerator;

        public GeneratePasswordController(IRandomStringGenerator randomStringGenerator)
        {
            _randomStringGenerator = randomStringGenerator;
        }


        /// <summary>
        /// Reached by GET to root "/GeneratePassword".
        /// Generates a random password, based on provided parameters.
        /// </summary>
        /// <param name="customsettings">Determines if the password should be generated using custom settings.</param>
        /// <param name="letters">Determines if the generated password should include letters</param>
        /// <param name="numbers">Determines if the generated password should include numbers</param>
        /// <param name="signs">Determines if the generated password should include special characters</param>
        /// <returns>GeneratePasswordResponse object containing the generated random password.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GeneratePassword(bool customsettings, bool letters, bool numbers, bool signs)
        {
            try
            {
                RandomStringRuleset randomStringRuleset;
                int passwordSize = 64;
                string randomlyGeneratedPassword = string.Empty;

                // The custom settings is added from the provided method parameters.
                if (customsettings)
                    randomStringRuleset = new RandomStringRuleset(passwordSize, letters, numbers, signs);
                else
                    randomStringRuleset = new RandomStringRuleset(passwordSize);

                // Generate the random password using the IRandomStringGenerator set in startup.
                randomlyGeneratedPassword = this._randomStringGenerator.GenerateRandomString(randomStringRuleset);

                return Ok(new GeneratePasswordResponse(randomlyGeneratedPassword));

            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                IncidentLogger.GetLogger.LogMessageAsync(IncidentLevel.CRITICAL, "An unexpected error occured while generating password.");
                return BadRequest(new ErrorResponse(errorMessage));
            }
        }

    }
}