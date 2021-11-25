using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing.SaltGenerators
{
    /// <summary>
    /// Class used to generate random salt values using the RNGCryptoServiceProvider class.
    /// </summary>
    class SaltGeneratorRNGCrypto : ISaltGenerator
    {
        /// <summary>
        /// Generate random salt value using RNGCryptoServiceProvider.
        /// The generated salt will have the length set in the saltLength argument.
        /// </summary>
        /// <param name="saltLength">Size of generated salt byte[]</param>
        /// <returns>Randomly generated salt byte[]</returns>
        public byte[] GenerateSalt(byte saltLength)
        {
            using (RNGCryptoServiceProvider saltMethod = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[saltLength];
                saltMethod.GetBytes(salt);
                return salt;
            }
        }
    }
}
