using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.CustomExceptions
{
    /// <summary>
    /// Custom exception thrown íf the a login attempt is made with wrong credentials.
    /// This is done in order to hide which information is wrong for the user.
    /// </summary>
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message): base(message)
        {
        }

        public InvalidCredentialsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
