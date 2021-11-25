using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing.SaltGenerators
{
    /// <summary>
    /// Contract used by classes generating salt values.
    /// </summary>
    internal interface ISaltGenerator
    {
        byte[] GenerateSalt(byte saltLength);
    }
}
