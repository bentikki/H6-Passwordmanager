using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing
{
    /// <summary>
    /// Contract used by HashingService implementations.
    /// </summary>
    public interface IHashingService
    {
        string GenerateHashedString(string inputString);
        bool CompareStringToHash(string valueToCompare, string originalHash);
    }
}
