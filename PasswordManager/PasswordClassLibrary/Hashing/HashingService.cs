using PasswordClassLibrary.Hashing.HashingMethod;
using PasswordClassLibrary.Hashing.SaltGenerators;
using System;

namespace PasswordClassLibrary.Hashing
{
    /// <summary>
    /// Facade used for hashing service functionality.
    /// Able to hash string input with randomly generated salt and provided pepper values.
    /// Salt generation is done by ISaltGenerator.
    /// Hashing is done by IHashingMethod.
    /// </summary>
    internal class HashingService : IHashingService
    {
        private readonly HashingSettings _hashingSettings;
        private readonly IHashingMethod _hashingMethod;

        // The max length of strings to be hashed - as per defined by NIST conventions.
        private readonly int _maxStringLength;

        /// <summary>
        /// Facade used for hashing service functionality. Able to hash string input.
        /// Settings used in the service is set in the provided HashingSettings class, this includes pepper string, size, and number of iterations.
        /// Throws: ArgumentException - if pepper value is empty, null, or whitespace.
        /// </summary>
        /// <param name="hashingSettings">Settings used to hash.</param>
        public HashingService(HashingSettings hashingSettings)
        {
            // Validate settings input.
            if (hashingSettings.PepperByteArray == null || hashingSettings.PepperByteArray.Length != 32)
                throw new ArgumentException("Provided pepper must have a length of 32 bits.", nameof(hashingSettings.PepperByteArray));

            // Use the provided settings.
            this._hashingSettings = hashingSettings;

            // Set the max input value based on hashing settings.
            this._maxStringLength = 188;

            // Set IHashingMethod from HashingMethodFactory
            this._hashingMethod = HashingMethodFactory.GetHashingMethod(this._hashingSettings.PepperByteArray, _hashingSettings.NumberOfIterations, _hashingSettings.KeyLength);
        }

        /// <summary>
        /// Returns a hashed string from the provided input string. The string will by in string format "salt:hash"
        /// Throws: ArgumentException if the provided string is null, empty, or whitespace only.
        /// Throws: ArgumentException if the provided string exceeds the max limit of allowed strings (64).
        /// </summary>
        /// <param name="inputString">Original string value to be hashed</param>
        /// <returns>Hashed string based on input - in format "salt:hash"</returns>
        public string GenerateHashedString(string inputString)
        {
            // Validate input
            if (string.IsNullOrEmpty(inputString) || string.IsNullOrWhiteSpace(inputString)) throw new ArgumentException("Then provided string must not be null or empty.", nameof(inputString));
            if (inputString.Length > _maxStringLength) throw new ArgumentException($"The provided string must not exceed a length of {_maxStringLength} characters.", nameof(inputString));

            try
            {
                // Get ISaltGenerator from factory - used to generate random salt value.
                ISaltGenerator saltGenerator = SaltGeneratorFactory.GetSaltGenerator();

                byte[] saltByteArray = saltGenerator.GenerateSalt(_hashingSettings.SaltLength); // Generate the salt value.
                byte[] hashByteArray = this._hashingMethod.GenerateHashFromString(inputString, saltByteArray); // Generate the hash value from inputString and salt.

                // Sets the hashed string based on inputString - in format salt:hash 
                string hashedAndSaltedString = Convert.ToBase64String(saltByteArray) + ":" + Convert.ToBase64String(hashByteArray);

                return hashedAndSaltedString;
            }
            catch (Exception e)
            {
                throw new HashingIncompleteException("An error occured during hashing.", e);
            }

        }

        /// <summary>
        /// Compares the provided raw string to the provided hashed string.  
        /// Throws: ArgumentException if the provided strings is null, empty, or whitespace only.
        /// Throws: ArgumentException if the provided valueToCompare string exceeds the max limit of allowed strings (64).
        /// </summary>
        /// <param name="valueToCompare"></param>
        /// <param name="originalHash"></param>
        /// <returns></returns>
        public bool CompareStringToHash(string valueToCompare, string originalHash)
        {
            // Validate input - neither of the provided string must be empty, null, or whitespace, nor must the valueToCompare exceed limit set in MaxStringLength.
            if (string.IsNullOrEmpty(valueToCompare) || string.IsNullOrWhiteSpace(valueToCompare)) throw new ArgumentException("Then provided valueToCompare string must not be null or empty.", nameof(valueToCompare));
            if (string.IsNullOrEmpty(originalHash) || string.IsNullOrWhiteSpace(originalHash)) throw new ArgumentException("Then provided originalHash string must not be null or empty.", nameof(originalHash));
            if (valueToCompare.Length > _maxStringLength) throw new ArgumentException($"The provided valueToCompare string must not exceed a length of {_maxStringLength} characters.", nameof(valueToCompare));

            try
            {
                // Call the CompareStringToHash method on the used IHashingMethod class - Return the resulting bool.
                return this._hashingMethod.CompareStringToHash(valueToCompare, originalHash);
            }
            catch (Exception e)
            {
                throw new HashingIncompleteException("An error occured during hashing.", e);
            }
        }

    }
}
