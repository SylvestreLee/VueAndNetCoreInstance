using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using GCP.Util.Extension;

namespace YiSha.Util
{
    public class LogHelper
    {
        private static readonly Logger log = LogManager.GetLogger(string.Empty);
        private static object lockHelper = new object();

        public static void Trace(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Trace(msg.ParseToString());
            }
            else
            {
                log.Trace(msg + GetExceptionMessage(ex));
            }
        }

        public static void Debug(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Debug(msg.ParseToString());
            }
            else
            {
                log.Debug(msg + GetExceptionMessage(ex));
            }
        }

        public static void Info(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Info(msg.ParseToString());
            }
            else
            {
                log.Info(msg + GetExceptionMessage(ex));
            }
        }

        public static void Warn(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Warn(msg.ParseToString());
            }
            else
            {
                log.Warn(msg + GetExceptionMessage(ex));
            }
        }

        public static void Error(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Error(msg.ParseToString());
            }
            else
            {
                log.Error(msg + GetExceptionMessage(ex));
            }
        }

        public static void Error(Exception ex)
        {
            if (ex != null)
            {
                log.Error(GetExceptionMessage(ex));
            }
        }

        public static void Fatal(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Fatal(msg.ParseToString());
            }
            else
            {
                log.Fatal(msg + GetExceptionMessage(ex));
            }
        }

        public static void Fatal(Exception ex)
        {
            if (ex != null)
            {
                log.Fatal(GetExceptionMessage(ex));
            }
        }

        private static string GetExceptionMessage(Exception ex)
        {
            string message = string.Empty;
            if (ex != null)
            {
                message += ex.Message;
                message += Environment.NewLine;
                Exception originalException = ex.GetOriginalException();
                if (originalException != null)
                {
                    if (originalException.Message != ex.Message)
                    {
                        message += originalException.Message;
                        message += Environment.NewLine;
                    }
                }
                message += ex.StackTrace;
                message += Environment.NewLine;
            }
            return message;
        }

        public static void Write(Exception ex)
        {
            string logContent = string.Empty;
            string logPath = DateTime.Now.ToString("yyyy-MM");
            logContent += GetExceptionMessage(ex);
            Write(logPath, logContent);
        }

        /// <summary>
        /// 写日志 异步
        /// 默认文件：yyyy-MM-dd.log
        /// </summary>
        /// <param name="logPath">日志目录[默认程序根目录\Log\下添加，故使用相对路径，如：营销任务]</param>
        /// <param name="logContent">日志内容 自动附加时间</param>
        public static void Write(string logPath, string logContent)
        {
            string logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            Write(logPath, logFileName, logContent);
        }

        /// <summary>
        /// 写日志 异步
        /// </summary>
        /// <param name="logPath">日志目录</param>
        /// <param name="logFileName">日志文件名</param>
        /// <param name="logContent">日志内容 自动附加时间</param>
        public static void Write(string logPath, string logFileName, string logContent)
        {
            if (string.IsNullOrWhiteSpace(logContent))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(logPath))
            {
                logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", DateTime.Now.ToString("yyyy-MM"));
            }
            else
            {
                logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", logPath.Trim('\\'));
            }
            if (string.IsNullOrWhiteSpace(logFileName))
            {
                logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            }
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            string fileName = Path.Combine(logPath, logFileName);
            Action taskAction = () =>
            {
                lock (lockHelper)
                {
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.WriteLine(logContent + Environment.NewLine);
                        sw.Flush();
                        sw.Close();
                    }
                }
            };
            Task task = new Task(taskAction);
            task.Start();
        }

        public static void Write(string logContent)
        {
            string logPath = DateTime.Now.ToString("yyyy-MM");
            Write(logPath, logContent);
        }

        public static void WriteWithTime(string logContent)
        {
            string logPath = DateTime.Now.ToString("yyyy-MM");
            logContent = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine + logContent;
            Write(logPath, logContent);
        }

        public static void WriteWithTime(Exception ex)
        {
            string logContent = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
            string logPath = DateTime.Now.ToString("yyyy-MM");
            logContent += GetExceptionMessage(ex);
            Write(logPath, logContent);
        }
    }
}
