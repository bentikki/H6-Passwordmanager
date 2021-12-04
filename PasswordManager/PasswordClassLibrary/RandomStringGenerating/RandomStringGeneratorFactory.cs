using PasswordClassLibrary.RandomStringGenerating.GenerationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.RandomStringGenerating
{
    public static class RandomStringGeneratorFactory
    {
        public static IRandomStringGenerator CreateGenerator(IRandomStringGenerationMethod randomStringGenerationMethod)
        {
            return new RandomStringGenerator(randomStringGenerationMethod);
        }
    }
}
