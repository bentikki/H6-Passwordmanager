using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing.HashingMethod
{
    /// <summary>
    /// Abstract master class used to hash strings.
    /// Must be inherited in concrete hashingmethod implementations.
    /// </summary>
    abstract class HashingMethodBase
    {
        protected readonly byte[] _pepper;
        protected readonly int _numberOfIteration;
        protected readonly int _keyLength;

        /// <summary>
        /// Base class used in hashing methods.
        /// Able to hash strings based on provided salt and pepper.
        /// </summary>
        /// <param name="pepper">Pepper value used in hashing.</param>
        /// <param name="numberOfIteration">Number of iterations the hashing algoritm should be repeated.</param>
        /// <param name="keyLength">Length of the generated hashed key.</param>
        protected HashingMethodBase(byte[] pepper, int numberOfIteration, byte keyLength)
        {
            _pepper = pepper;
            _numberOfIteration = numberOfIteration;
            _keyLength = keyLength;
        }

        /// <summary>
        /// Abstract method must be implemented in all classes derived from HashingMethodBase.
        /// Contains the logic regarding the specific hashing method run.
        /// </summary>
        /// <param name="stringToHash">Original string to hash.</param>
        /// <param name="salt">The salt to hash the string with.</param>
        /// <returns>The hashed value of the string as byte[]</returns>
        public abstract byte[] GenerateHashFromString(string stringToHash, byte[] salt);

        /// <summary>
        /// Method to compare raw string to the provided hashed value.
        /// If the hashed value of the valueToCompare string, mathes the value of the originalHash hashed value - return True, else return false.
        /// </summary>
        /// <param name="valueToCompare">Raw string value to hash and compare.</param>
        /// <param name="originalHash">Hashed value to be compared to.</param>
        /// <returns>Boolean true if the hashed values match, else false.</returns>
        public virtual bool CompareStringToHash(string valueToCompare, string originalHash)
        {
            // Split the provided hash, in order to get 
            string[] hashParts = originalHash.Split(':');
            byte[] salt = Convert.FromBase64String(hashParts[0]);
            byte[] originalHashPart = Convert.FromBase64String(hashParts[1]);
            byte[] hashToCompare = GenerateHashFromString(valueToCompare, salt);

            // An extra step used to defend against timing attack.
            // - this is an function found online, not self made.
            // 
            //The following is required to defend against timing attacks.
            //We dont want to exit at the first byte mismatch but test every byte no matter what
            //so that timing is the same for valid and invalid passwords and
            //we dont want to leak information to the attacker about upto which byte his guess is correct.
            uint differences = (uint)originalHashPart.Length ^ (uint)hashToCompare.Length;
            for (int position = 0; position < Math.Min(originalHashPart.Length, hashToCompare.Length); position++)
                differences |= (uint)(originalHashPart[position] ^ hashToCompare[position]);
            return differences == 0;
        }
    }
}
