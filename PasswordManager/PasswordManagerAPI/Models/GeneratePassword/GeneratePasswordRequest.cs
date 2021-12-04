using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models.GeneratePassword
{
    public class GeneratePasswordRequest
    {
        public bool Customsettings { get; set; }
        public bool Letters { get; set; }
        public bool Numbers { get; set; }
        public bool Signs { get; set; }
    }
}
