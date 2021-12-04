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

namespace PasswordManagerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class GeneratePasswordController : CustomControllerBase
    {
        IRandomStringGenerator _randomStringGenerator;

        public GeneratePasswordController(IRandomStringGenerator randomStringGenerator)
        {
            _randomStringGenerator = randomStringGenerator;
        }



        // Reached by GET to root "/GeneratePassword"
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

                if (customsettings)
                    randomStringRuleset = new RandomStringRuleset(passwordSize, letters, numbers, signs);
                else
                    randomStringRuleset = new RandomStringRuleset(passwordSize);

                randomlyGeneratedPassword = this._randomStringGenerator.GenerateRandomString(randomStringRuleset);

                return Ok(new GeneratePasswordResponse(randomlyGeneratedPassword));

            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
        }

    }
}