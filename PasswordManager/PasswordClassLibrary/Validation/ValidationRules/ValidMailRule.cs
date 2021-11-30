using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation.ValidationRules
{
    public class ValidMailRule : IValidationRule
    {
        public void CheckRule(string valueName, object value)
        {
            if (value is string stringVal)
            {
                // This will throw an ArgumentException if the given mail is invalid.
                // This follows the email standard given by RFC#822: https://www.ietf.org/rfc/rfc0822.txt
                // Which means an email called mail@com IS valid.
                try
                {
                    MailAddress m = new MailAddress(stringVal);
                }
                catch (Exception)
                {
                    string message = $"{stringVal} is not a valid {valueName}";

                    throw new ArgumentException(message);
                }
            }
        }
    }
}
