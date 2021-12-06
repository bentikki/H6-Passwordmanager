using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Logging.LoggingMethods
{
    /// <summary>
    /// Error logging via Email
    /// </summary>
    internal class MailLogger : LoggingMaster
    {
        public MailLogger(IncidentLevel level) : base(level) { }

        protected override async Task WriteAsync(string message, Exception exception)
        {
            try
            {
                string errorLogMail = this.GetErrorToMail();
                var client = this.GetErrorSmtpClient();

                string currentTimestamp = DateTime.Now.ToString(@"MM/dd/yyyy HH:mm:ss");
                string mailSubject = $"{currentTimestamp} - An error occured";

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"A critical error has occured at:");
                stringBuilder.Append($"{currentTimestamp} : {message}");
                if (exception != null)
                {
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append("See Exception below:");
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append($"{currentTimestamp} : {exception.GetType().Name} - {exception.Message}");
                }
                stringBuilder.Append(Environment.NewLine);

                client.Send(errorLogMail, errorLogMail, mailSubject, stringBuilder.ToString());
            }
            catch (Exception e) { }
        }



        /// <summary>
        /// SmtpClient used for mail error logging.
        /// </summary>
        /// <returns>SmtpClient used for mail error logging</returns>
        private SmtpClient GetErrorSmtpClient()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("BTOsignUpAPI@gmail.com", "mRBLaY9zT5hNt8FinzI8uZkT1"),
                EnableSsl = true
            };

            return client;
        }

        /// <summary>
        /// Gets the mail address to send that shall recieve error logging.
        /// </summary>
        /// <returns>Mail address used to recieve error logs.</returns>
        private string GetErrorToMail()
        {
            string errorLogginToMail = "BTOsignUpAPI@gmail.com";
            return errorLogginToMail;
        }

    }
}