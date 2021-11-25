using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.CustomExceptions
{
    /// <summary>
    /// Custom exception thrown íf the Data Access Object is not available from the repository.
    /// This could be if the API is not available, or if the Database could not be reached.
    /// </summary>
    public class RepositoryNotAvailableException : Exception
    {
        public RepositoryNotAvailableException(string message): base(message)
        {
        }

        public RepositoryNotAvailableException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
