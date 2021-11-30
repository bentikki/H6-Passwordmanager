using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation.ValidationRules
{
    public class MinValueRule : IValidationRule
    {
        private int minValue;

        public MinValueRule(int minValue)
        {
            this.minValue = minValue;
        }

        public void CheckRule(string valueName, object value)
        {
            if(value is int valueInt)
            {
                if (valueInt < minValue)
                {
                    throw new ArgumentException($"{valueName} must be at least {minValue}.");
                }
            }
        }
    }
}
