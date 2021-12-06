using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Logging.LoggingMethods
{
    /// <summary>
    /// Error logging via Local file
    /// </summary>
    internal class LocalFileLogger : LoggingMaster
    {
        private static ReaderWriterLockSlim lock_ = new ReaderWriterLockSlim();


        public LocalFileLogger(IncidentLevel level) : base(level) { }

        protected override async Task WriteAsync(string message, Exception exception)
        {
            string filePath = @"ErrorLog/LogFile.txt";
            lock_.EnterWriteLock();

            try
            {
                string currentTimestamp = DateTime.Now.ToString(@"dd/MM/yyyy HH:mm:ss");
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"{currentTimestamp} : {message}");
                if(exception != null)
                {
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append("See Exception below:");
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append($"{currentTimestamp} : {exception.GetType().Name} - {exception.Message}");
                }
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);

                FileInfo fi = new FileInfo(filePath);
                if (!fi.Directory.Exists)
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }

                using (StreamWriter outputFile = new StreamWriter(filePath, true))
                {
                    await outputFile.WriteAsync(stringBuilder.ToString());
                }
            }
            catch (Exception e) { }
            finally
            {
                lock_.ExitWriteLock();
            }
        }

    }
}
    