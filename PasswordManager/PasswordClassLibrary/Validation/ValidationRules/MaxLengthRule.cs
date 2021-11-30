using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation.ValidationRules
{
    public class MaxLengthRule : IValidationRule
    {
        private int maxLength;

        public MaxLengthRule(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public void CheckRule(string valueName, object value)
        {
            if(value is string valueString)
            {
                if (valueString.Length > maxLength)
                {
                    throw new ArgumentException($"{valueName} must not be longer than {maxLength} characters.");
                }
            }
        }
    }
}
