using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation.ValidationRules
{
    public class MinLengthRule : IValidationRule
    {
        private int minLength;

        public MinLengthRule(int maxLength)
        {
            this.minLength = maxLength;
        }

        public void CheckRule(string valueName, object value)
        {
            if(value is string valueString)
            {
                if (valueString.Length < minLength)
                {
                    throw new ArgumentException($"{valueName} must not be shorter than {minLength} characters.");
                }
            }
        }
    }
}
