using PasswordClassLibrary.Hashing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PasswordClassLibrary.Tests
{
    public class HashingServiceTests
    {
        private readonly IHashingService _hashingService;

        // A random pepper string is used for testing.
        private const string PepperString = "15 13 0f 35 ae 89 4c ed ed f4 82 64 b0 4b 0b a2 a3 e3 a2 b9 56 e1 27 18 11 a9 e1 77 f1 3c b8 64";

        public HashingServiceTests()
        {
            // Set the service to be used during testing.
            HashingSettings hashingSettings = new HashingSettings(ConvertSpacedHexToByteArray(PepperString));
            this._hashingService = HashingServiceFactory.GetHashingService(hashingSettings);
        }

        [Theory]
        [InlineData("testString1234")]
        [InlineData("t")]
        [InlineData("13huqkjwbekjqwbeb1hkj2g3k1b23mn mn nmqwrq qwe qwe ")]
        [InlineData("hhrorompshaylgbbqllrbhbfchslgsyeiqyzwzidmffckyjhclzbfmxwhzbsjhgy")] // Test string with 64 characters.
        public void GenerateHashedString_ValidString_ShouldReturnHashedString(string originalStringInput)
        {
            // Arrange
            string originalString = originalStringInput;

            // Act
            string hashedString = this._hashingService.GenerateHashedString(originalString);

            // Assert
            Assert.NotNull(hashedString);
            Assert.False(string.IsNullOrEmpty(hashedString));
            Assert.False(string.IsNullOrWhiteSpace(hashedString));
            Assert.NotEqual(hashedString, originalString);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                     ")]
        public void GenerateHashedString_InvalidInput_ShouldThrowArgumentException(string originalStringInput)
        {
            // Arrange
            string originalString = originalStringInput;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => this._hashingService.GenerateHashedString(originalString));
        }

        [Theory]
        [InlineData("hhrorompshaylgbbqllrbhbfchslgsyeiqyzwzidmffckyjhclzbfmxwhzbsjhgyshhrorompshaylgbbqllrbhbfchslgsyeiqyzwzidmffckyjhclzbfmxwhzbsjhgysquyhghhrorompshaylgbbqllrbhbfchslgsyeiqyzwzidmffckyjhclzbfmxwhzbsjhgysquyhg")] // Test string with 65 characters.
        [InlineData("hhrorompshaylgbbqllrbhbfchslgsyeiqyzwzidmffckyjhclzbfmxwhzbsjhgysquyhghhrorompshaylgbbqllrbhbfchslgsyeiqyzwzidmffckyjhclzbfmxwhzbsjhgysquyhghhrorompshaylgbbqllrbhbfchslgsyeiqyzwzidmffckyjhclzbfmxwhzbsjhgysquyhg")] // Test string with 70 characters.
        public void GenerateHashedString_ExceedingMaxLength_ShouldThrowArgumentException(string originalStringInput)
        {
            // Arrange
            string originalString = originalStringInput;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => this._hashingService.GenerateHashedString(originalString));
        }

        [Fact]
        public void CompareStringToHash_IdenticalOriginal_ShouldReturnTrue()
        {
            // Arrange
            string originalPassword = "testpassword1234";
            string originalHash = this._hashingService.GenerateHashedString(originalPassword);

            // Act
            bool hashIsIdentical = this._hashingService.CompareStringToHash(originalPassword, originalHash);

            // Assert
            Assert.True(hashIsIdentical);
        }

        [Fact]
        public void CompareStringToHash_DiffrentFromOriginal_ShouldReturnFalse()
        {
            // Arrange
            string originalPassword = "testpassword1234";
            string diffrentPassword = originalPassword + "addedtest";
            string originalHash = this._hashingService.GenerateHashedString(originalPassword);

            // Act
            bool hashIsIdentical = this._hashingService.CompareStringToHash(diffrentPassword, originalHash);

            // Assert
            Assert.False(hashIsIdentical);
        }


        /// <summary>
        /// hexString should have its double hex digits corresponding to each byte separated by ' '
        /// like so: "A1 23 F3 14".
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns>byte[] form</returns>
        private byte[] ConvertSpacedHexToByteArray(string hexString)
        {
            string[] hexValuesSplit = hexString.Split(' ');
            byte[] data = new byte[hexValuesSplit.Length];

            for (var index = 0; index < hexValuesSplit.Length; index++)
            {
                data[index] = byte.Parse(hexValuesSplit[index], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

    }
}
