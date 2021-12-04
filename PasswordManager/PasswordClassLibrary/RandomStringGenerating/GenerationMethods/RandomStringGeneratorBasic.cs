using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.RandomStringGenerating.GenerationMethods
{
    public class RandomStringGeneratorBasic : IRandomStringGenerationMethod
    {
        public string GenerateRandomString(RandomStringRuleset stringRuleSet)
        {
            Random random = new Random();

            string generatedRandomString = string.Empty;
            string allowedChars = string.Empty;

            if (!stringRuleSet.IncludeCharacters && !stringRuleSet.IncludeNumbers && !stringRuleSet.IncludeSpecialchars)
            {
                allowedChars += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            }
            else
            {
                if (stringRuleSet.IncludeCharacters) allowedChars += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                if (stringRuleSet.IncludeNumbers) allowedChars += "0123456789";
                if (stringRuleSet.IncludeSpecialchars) allowedChars += "-~()'!*:@;";
            }

            for (int i = 0; i < stringRuleSet.StringSize; i++)
            {
                generatedRandomString += allowedChars[random.Next(allowedChars.Length)];
            }

            return generatedRandomString;
        }
    }
}
