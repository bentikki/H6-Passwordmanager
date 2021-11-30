using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation.ValidationRules
{
    public class MustHaveMailDomainRule : IValidationRule
    {
        private string mustHaveDomain;

        public MustHaveMailDomainRule(string mustHaveDomain)
        {
            this.mustHaveDomain = mustHaveDomain;
        }

        public void CheckRule(string valueName, object value)
        {
            if (value is string stringVal)
            {
                // This will throw an ArgumentException if the given mail is invalid.
                // This follows the email standard given by RFC#822: https://www.ietf.org/rfc/rfc0822.txt
                // Which means an email called mail@com IS valid.
                string mailErrorMessage = $"The mail must contain the {mustHaveDomain} domain";

                MailAddress mail;
                try
                {
                    mail = new MailAddress(stringVal);

                }
                catch (Exception)
                {
                    throw new ArgumentException(mailErrorMessage, nameof(valueName));
                }

                if (mail.Host.ToUpper() != mustHaveDomain.ToUpper())
                    throw new ArgumentException(mailErrorMessage, nameof(valueName));
            }
        }
    }
}
