using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.RandomStringGenerating
{
    public class RandomStringRuleset
    {
        public bool IncludeCharacters { get; set; }
        public bool IncludeNumbers { get; set; }
        public bool IncludeSpecialchars { get; set; }
        public int StringSize { get; set; }

        public RandomStringRuleset(int stringSize) : this(stringSize, true, true, true) { }

        public RandomStringRuleset(int stringSize, bool includeCharacters, bool includeNumbers, bool includeSpecialchars)
        {
            IncludeCharacters = includeCharacters;
            IncludeNumbers = includeNumbers;
            IncludeSpecialchars = includeSpecialchars;
            StringSize = stringSize;
        }
    }
}
