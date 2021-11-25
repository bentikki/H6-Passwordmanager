using PasswordClassLibrary.Hashing.HashingMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing
{
    /// <summary>
    /// Factory to generate the used IHashingMethod
    /// </summary>
    internal static class HashingMethodFactory
    {
        public static IHashingMethod GetHashingMethod(byte[] pepper, int numberOfIteration, byte keyLength)
        {
            return new HashingMethodPBKDF2(pepper, numberOfIteration, keyLength);
        }
    }
}
