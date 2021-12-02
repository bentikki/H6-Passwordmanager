using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.GeneratePassword
{
    public class GeneratePasswordResponse
    {
        public string GeneratedPassword { get; set; }

        public GeneratePasswordResponse(string generatedPassword)
        {
            GeneratedPassword = generatedPassword;
        }
    }
}
