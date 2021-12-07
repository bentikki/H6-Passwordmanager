using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.RandomStringGenerating
{
    public class RandomStringRuleset
    {
        public bool IncludeCharacters { get; private set; }
        public bool IncludeNumbers { get; private set; }
        public bool IncludeSpecialchars { get; private set; }
        public int StringSize { get; private set; }

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
