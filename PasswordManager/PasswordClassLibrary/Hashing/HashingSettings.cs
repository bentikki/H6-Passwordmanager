using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing
{
    /// <summary>
    /// Contains the settings used in the HashingService.
    /// </summary>
    public class HashingSettings
    {
        /// <summary>
        /// The pepper value to be used used in hashing.
        /// This value should not be stored at the same place as the finished hash.
        /// </summary>
        public byte[] PepperByteArray { get; }
        public byte SaltLength { get; }
        public byte KeyLength { get; }

        /// <summary>
        /// The number of times the hash should be repeated.
        /// The reccommended value is 50.000.
        /// This should not be below 10.000.
        /// </summary>
        public int NumberOfIterations { get; }

        /// <summary>
        /// Settings used in the IHashingService.
        /// A provided byte array containing the pepper bytes, must be provided. 
        /// Optionally saltLength, keyLength, and numberOfIterations can be set, else they will use the values recommended by NIST.
        /// Reference: https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#pbkdf2
        /// </summary>
        /// <param name="pepperByteArray">Required 32 byte array with pepper values.</param>
        /// <param name="saltLength">Length of randomly generated salt - recommended value: 64</param>
        /// <param name="keyLength">Length of hashed key value - recommended value: 64</param>
        /// <param name="numberOfIterations">Number of iterations the hashing method will be run - recommended 120.000</param>
        public HashingSettings(byte[] pepperByteArray, byte saltLength = 64, byte keyLength = 64, int numberOfIterations = 120000)
        {
            PepperByteArray = pepperByteArray;
            SaltLength = saltLength;
            KeyLength = keyLength;
            NumberOfIterations = numberOfIterations;
        }
    }
}
