using PasswordClassLibrary.Logging.LoggingMethods;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PasswordClassLibrary.Logging
{
    /// <summary>
    /// Error logging class.
    /// Uses Chain of Responsibility pattern.
    /// </summary>
    public static class IncidentLogger
    {
        private static LoggingMaster loggerChain;

        /// <summary>
        /// Returns the chain of loggers used in the error logging.
        /// </summary>
        public static LoggingMaster GetLogger
        {
            get
            {
                if (loggerChain == null)
                    SetLoggerChain();

                return loggerChain;
            }
        }
        /// <summary>
        /// Sets the chain of loggers used in the error logging.
        /// </summary>
        /// <returns>LoggingMaster object containing the chain.</returns>
        private static void SetLoggerChain()
        {
            LoggingMaster fileLogger = new LocalFileLogger(IncidentLevel.MINOR);
            LoggingMaster dbLogger = new DbLogger(IncidentLevel.MAJOR);
            LoggingMaster mailLogger = new MailLogger(IncidentLevel.CRITICAL);

            fileLogger
                .Next(dbLogger)
                .Next(mailLogger);

            loggerChain = fileLogger;
        }


    }
}
