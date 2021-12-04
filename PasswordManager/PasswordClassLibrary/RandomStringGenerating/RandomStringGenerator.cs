using PasswordClassLibrary.RandomStringGenerating.GenerationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.RandomStringGenerating
{
    internal class RandomStringGenerator : IRandomStringGenerator
    {
        private IRandomStringGenerationMethod generationMethod;

        public RandomStringGenerator(IRandomStringGenerationMethod generationMethod)
        {
            this.generationMethod = generationMethod;
        }

        public string GenerateRandomString(RandomStringRuleset stringRuleset)
        {
            return this.generationMethod.GenerateRandomString(stringRuleset);
        }
    }
}
