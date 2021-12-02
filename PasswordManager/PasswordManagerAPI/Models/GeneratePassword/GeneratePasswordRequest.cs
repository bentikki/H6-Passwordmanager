using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.GeneratePassword
{
    public class GeneratePasswordRequest
    {
        public bool IncludeLetters { get; set; }
        public bool IncludeNumbers { get; set; }
        public bool IncludeSigns { get; set; }
    }
}
