using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Hashing
{
    /// <summary>
    /// Custom exception thrown íf an error occured during hashing.
    /// </summary>
    public class HashingIncompleteException : Exception
    {
        public HashingIncompleteException(string message): base(message)
        {
        }

        public HashingIncompleteException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
