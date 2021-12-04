using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.RandomStringGenerating.GenerationMethods
{
    public interface IRandomStringGenerationMethod
    {
        string GenerateRandomString(RandomStringRuleset stringRuleSet);
    }
}
