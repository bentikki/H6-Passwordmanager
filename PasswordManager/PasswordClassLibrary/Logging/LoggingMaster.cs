using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Logging
{
    /// <summary>
    /// Enum to represent the level of importance of logged message.
    /// </summary>
    public enum IncidentLevel
    {
        MINOR = 1,
        MAJOR = 2,
        CRITICAL = 3
    }

    /// <summary>
    /// Abstract logging class used for inheritanse in concrete logging implementations.
    /// </summary>
    public abstract class LoggingMaster
    {
        protected IncidentLevel level;
        protected LoggingMaster nextLogger;
        private static List<Task> logginTasks = new List<Task>();

        protected LoggingMaster(IncidentLevel level)
        {
            this.level = level;
        }

        public LoggingMaster Next(LoggingMaster nextLogger)
        {
            this.nextLogger = nextLogger;
            return this.nextLogger;
        }

        public async Task LogMessageAsync(IncidentLevel level, string message, Exception exception)
        {
            // Check if the logging level sat is below or equal.
            if (this.level <= level)
            {
                logginTasks.Add(WriteAsync(message, exception));
            }

            // Check if another logger is set in the chain.
            if (nextLogger != null)
            {
                await nextLogger.LogMessageAsync(level, message, exception);
            }

            if(logginTasks.Count > 0)
            {
                Task.WaitAll(logginTasks.ToArray());
                logginTasks.Clear();
            }
        }

        public async Task LogMessageAsync(IncidentLevel level, string message)
        {
            await this.LogMessageAsync(level, message, null);
        }

        protected abstract Task WriteAsync(string message, Exception exception);
    }
}
