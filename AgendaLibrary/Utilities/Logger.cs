using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaLibrary.Utilities
{
    public class Logger
    {
        StreamWriter logger_writer;
        string log_severity = "";
        // logger initialization
        public Logger(string filename) 
        { 
            this.logger_writer = new StreamWriter($"{Environment.CurrentDirectory}\\{filename}",true);
            this.logger_writer.AutoFlush = true;
        }
        // logging functions
        public void LogInformation(string content) 
        {
            log_severity = "INFORMATION";
            WriteToLog(content, log_severity);
        }
        public void LogWarning(string content)
        {
            log_severity = "WARNING";
            WriteToLog(content, log_severity);
        }
        public void LogError(string content)
        {
            log_severity = "ERROR";
            WriteToLog(content, log_severity);
        }
        // internal function
        internal void WriteToLog(string content, string severity)
        {
            logger_writer.WriteLine($"{DateTime.Now} - {severity} - {content}");
        }
    }
}
