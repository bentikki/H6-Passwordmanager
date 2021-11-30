using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation.ValidationRules
{
    public class NoNullRule : IValidationRule
    {
        public void CheckRule(string valueName, object value)
        {
            if(value == null)
            {
                throw new ArgumentNullException($"{valueName} must not be null.");
            }
        }
    }
}
