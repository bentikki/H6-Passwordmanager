using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation.ValidationRules
{
    public class NoEmptyStringRule : IValidationRule
    {
        public void CheckRule(string valueName, object value)
        {
            if(value is string valueString)
            {
                if (string.IsNullOrEmpty(valueString) || string.IsNullOrWhiteSpace(valueString) || valueString == "")
                {
                    throw new ArgumentException($"{valueName} must not be empty.");
                }
            }
        }
    }
}
