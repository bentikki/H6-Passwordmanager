using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Logging.LoggingMethods
{
    /// <summary>
    /// Error logging via Database Table
    /// </summary>
    internal class DbLogger : LoggingMaster
    {
        public DbLogger(IncidentLevel level) : base(level) { }

        protected override async Task WriteAsync(string message, Exception exception)
        {
            if (exception is SqlException) return;

            try
            {
                string exceptionType = string.Empty;
                string exceptionMessage = string.Empty;

                if(exception != null)
                {
                    exceptionType = exception.GetType().Name;
                    exceptionMessage = exception.Message;
                }

                using (var conn = this.GetSqlConnectionErrorLogger())
                {
                    await conn.OpenAsync();

                    // Execute stored procedure to create new error log
                    var procedure = "[SP_LogError]";
                    var values = new
                    {
                        @ErrorMessage = message,
                        @ExceptionType = exceptionType,
                        @ExceptionMessage = exceptionMessage
                    };
                    await conn.ExecuteAsync(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e) { }
        }

        /// <summary>
        /// SqlConnection with permission to log errors via SPLogError
        /// </summary>
        /// <returns>SqlConnection with specific permission</returns>
        private SqlConnection GetSqlConnectionErrorLogger()
        {
            string username = "ErrorLoggingUser";
            string password = "Passw0rd";

            return new SqlConnection($"Server=172.16.57.4;Database=PasswordManagerMain;User Id={username};Password={password}");

        }

    }
}