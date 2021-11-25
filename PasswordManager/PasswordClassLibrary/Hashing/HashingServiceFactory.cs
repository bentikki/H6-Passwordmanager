using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing
{
    /// <summary>
    /// Factory used to generate IHashingService instances.
    /// </summary>
    public static class HashingServiceFactory
    {
        /// <summary>
        /// Generate IHashingService without providing HashingSettings.
        /// </summary>
        /// <param name="PepperByteArray">Byte array containing the pepper value.</param>
        /// <returns>Instance of used IHashingService.</returns>
        public static IHashingService GetHashingService(byte[] PepperByteArray)
        {
            HashingSettings hashingSettings = new HashingSettings(PepperByteArray);
            return HashingServiceFactory.GetHashingService(hashingSettings);
        }

        /// <summary>
        /// Generate IHashingService with provided HashingSettings.
        /// </summary>
        /// <param name="hashingSettings">HashingSettings used in the IHashingService.</param>
        /// <returns>Instance of used IHashingService</returns>
        public static IHashingService GetHashingService(HashingSettings hashingSettings)
        {
            return new HashingService(hashingSettings);
        }
    }
}
