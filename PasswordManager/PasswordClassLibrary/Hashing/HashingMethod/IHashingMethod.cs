using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing.HashingMethod
{
    /// <summary>
    /// Interface contract of hashing methods.
    /// </summary>
    interface IHashingMethod
    {
        byte[] GenerateHashFromString(string stringToHash, byte[] salt);
        bool CompareStringToHash(string valueToCompare, string originalValue);
    }
}
