using PasswordClassLibrary.Hashing.SaltGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing
{
    /// <summary>
    /// Factory used to generate ISaltGenerator instances.
    /// </summary>
    internal static class SaltGeneratorFactory
    {
        public static ISaltGenerator GetSaltGenerator()
        {
            return new SaltGeneratorRNGCrypto();
        }
    }
}
