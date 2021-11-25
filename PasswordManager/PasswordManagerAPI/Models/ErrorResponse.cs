using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerAPI.Models
{
    /// <summary>
    /// Model used as API error response.
    /// </summary>
    public class ErrorResponse
    {
        public string Error { get; private set; }

        public ErrorResponse(string error)
        {
            Error = error;
        }
    }
}
