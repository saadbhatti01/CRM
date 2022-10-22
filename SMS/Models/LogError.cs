using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogError
{
    public class LogError
    {
        public static void Write(Exception ex)
        {
            try
            {
                string logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PressPoint\\";
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                logFilePath = logFilePath + DateTime.Now.ToString("dd-MM-yyyy") + ".log";
                File.AppendAllText(logFilePath, "Time: " + DateTime.Now.ToString("hh:mm:ss tt") + "\r\nError Message: " + ex.Message + "\r\nInner Exception 1: " + (ex.InnerException != null ? ex.InnerException.Message + "\r\nInner Exception 2: " + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message + "\r\nInner Exception 3: " + (ex.InnerException.InnerException.InnerException != null ? ex.InnerException.InnerException.InnerException.Message : "null") : "null") : "null") + "\r\nStack Trace:\r\n" + ex.StackTrace + "\r\n\r\n\r\n");
            }
            catch (Exception exe)
            {
                string logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PressPoint\\";
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                logFilePath = logFilePath + DateTime.Now.ToString("dd-MM-yyyy") + ".log";
                File.AppendAllText(logFilePath, "Time: " + DateTime.Now.ToString("hh:mm:ss tt") + "\r\nError Message: " + exe.Message + "\r\nInner Exception 1: " + (exe.InnerException != null ? exe.InnerException.Message + "\r\nInner Exception 2: " + (exe.InnerException.InnerException != null ? exe.InnerException.InnerException.Message + "\r\nInner Exception 3: " + (exe.InnerException.InnerException.InnerException != null ? exe.InnerException.InnerException.InnerException.Message : "null") : "null") : "null") + "\r\nStack Trace:\r\n" + exe.StackTrace + "\r\n\r\n\r\n");
            }
        }

        public static void WriteReplicaLog(string text)
        {
            try
            {
                string logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PressPoint\\";
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                logFilePath = logFilePath +"Replica"+ DateTime.Now.ToString("dd-MM-yyyy") + ".log";
                File.AppendAllText(logFilePath, "Time: " + DateTime.Now.ToString("hh:mm:ss tt") +"\r\n" +text);
            }
            catch (Exception exe)
            {
                string logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PressPoint\\";
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                logFilePath = logFilePath + DateTime.Now.ToString("dd-MM-yyyy") + ".log";
                File.AppendAllText(logFilePath, "Time: " + DateTime.Now.ToString("hh:mm:ss tt") + "\r\nError Message: " + exe.Message + "\r\nInner Exception 1: " + (exe.InnerException != null ? exe.InnerException.Message + "\r\nInner Exception 2: " + (exe.InnerException.InnerException != null ? exe.InnerException.InnerException.Message + "\r\nInner Exception 3: " + (exe.InnerException.InnerException.InnerException != null ? exe.InnerException.InnerException.InnerException.Message : "null") : "null") : "null") + "\r\nStack Trace: " + exe.StackTrace + "\r\n\r\n\r\n");
            }
        }
    }
}
