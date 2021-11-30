using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation
{
    /// <summary>
    /// Contract for validation rules, used to contain logic by which to validate a given value.
    /// </summary>
    public interface IValidationRule
    {
        /// <summary>
        /// Validate the given value against the rules logic.
        /// This should throw an exception if the value does not pass.
        /// </summary>
        /// <param name="valueName">The name of the value to validate - this is used to return better error messages.</param>
        /// <param name="value">The value to check.</param>
        void CheckRule(string valueName, object value);
    }
}
