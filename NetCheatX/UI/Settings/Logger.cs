using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NetCheatX.UI.Settings
{
    public class Logger
    {
        private string _filePath = null;
        private string _directory = null;
        private DateTime _startTime;

        public Logger(string directory, string preFileName, DateTime start)
        {
            int x = 0;

            _filePath = Path.Combine(directory, preFileName + " " + start.ToShortDateString().Replace("/", "-") + "-" + start.ToLocalTime().ToString("HH-mm-ss") + ".log");
            _directory = directory;
            _startTime = start;

            // Setup workspace
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);

            // Ensure we aren't overwriting an existimg log file (shouldn't be possible)
            while (File.Exists(_filePath))
            {
                _filePath = Path.Combine(directory, preFileName + " " + start.ToShortDateString().Replace("/", "-") + "-" + start.ToLocalTime().ToString("HH-mm-ss") + "." + x.ToString() + ".log");
                x++;
            }
        }

        // Add an exception log
        public void LogException(Exception log, string context = "UI")
        {
            string depth = " ";

            StreamWriter sw = SetupLog();
            if (sw == null || log == null)
                return;

            sw.WriteLine("EXCEPTION [" + context + "] " + GetTime() + " : ");

            while (log != null)
            {
                sw.WriteLine(depth + "StackTrace: " + log.StackTrace + "\r\n" +
                    depth + "Source:" + log.Source + "\r\n" +
                    depth + "Message: " + log.Message + "\r\n");

                log = log.InnerException;
                depth += " ";
            }

            sw.Close();
        }

        // Add a string log
        public void LogString(string log, string context = "UI")
        {
            StreamWriter sw = SetupLog();
            if (sw == null)
                return;

            sw.WriteLine("STRING [" + context + "] " + GetTime() + " : " + log);
            sw.Close();
        }


        private StreamWriter SetupLog()
        {
            if (_filePath == null || _filePath == "")
                return null;

            try
            {
                if (!Directory.Exists(_directory))
                    Directory.CreateDirectory(_directory);

                return File.AppendText(_filePath);
            }
            catch { return null; }
        }

        private string GetTime()
        {
            TimeSpan now = DateTime.Now - _startTime;

            return now.ToString();
        }
    }
}
