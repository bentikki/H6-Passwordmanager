using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing.HashingMethod
{
    /// <summary>
    /// Hashing method using the PBKDF2 implementation recommended by NIST.
    /// https://auth0.com/blog/dont-pass-on-the-new-nist-password-guidelines/
    /// </summary>
    internal class HashingMethodPBKDF2 : HashingMethodBase, IHashingMethod
    {
        /// <summary>
        /// Hashing method using the PBKDF2 algoritm.
        /// Able to hash strings based on provided salt and pepper.
        /// </summary>
        /// <param name="pepper">Pepper value used in hashing.</param>
        /// <param name="numberOfIteration">Number of iterations the hashing algoritm should be repeated.</param>
        /// <param name="keyLength">Length of the generated hashed key.</param>
        public HashingMethodPBKDF2(byte[] pepper, int numberOfIteration, byte keyLength) : base(pepper, numberOfIteration, keyLength) { }

        /// <summary>
        /// Hashing method using the PBKDF2 algoritm.
        /// Contains the logic regarding the specific hashing method run.
        /// </summary>
        /// <param name="stringToHash">Original string to hash.</param>
        /// <param name="salt">The salt to hash the string with.</param>
        /// <returns>The hashed value of the string as byte[]</returns>
        public override byte[] GenerateHashFromString(string stringToHash, byte[] salt)
        {
            //Create an hmac hash of the password using the pepper value as the key
            using (var hmac = new HMACSHA512(this._pepper))
            {
                byte[] initialHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

                //Generate a key value using PBKDF2 that will serve as the password hash
                using (var pbkdf2 = new Rfc2898DeriveBytes(initialHash, salt, _numberOfIteration))
                {
                    return pbkdf2.GetBytes(this._keyLength);
                }
            }
        }
    }
}
