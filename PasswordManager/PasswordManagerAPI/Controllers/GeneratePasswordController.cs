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

namespace PasswordManagerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class GeneratePasswordController : CustomControllerBase
    {


        public GeneratePasswordController()
        {

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
                string saltString = string.Empty;


                if (customsettings) saltString = this.RandomString(letters, numbers, signs);
                else saltString = this.RandomString(true,true,true);

                return Ok(new GeneratePasswordResponse(saltString));
            }
            catch (Exception e)
            {
                string errorMessage = e.Message;
                return BadRequest(new ErrorResponse(errorMessage));
            }
        }

        private Random random = new Random();

        public string RandomString(bool letters, bool numbers, bool signs)
        {
            string chars = string.Empty;

            if (!letters && !numbers && !signs)
            {
                chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            }
            else
            {
                if (letters) chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                if (numbers) chars += "0123456789";
                if (signs) chars += "!#%@£$+?'*,.-_";
            }

            return new string(Enumerable.Repeat(chars, 64)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}