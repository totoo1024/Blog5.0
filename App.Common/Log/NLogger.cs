using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Log
{
    /// <summary>
    /// 日志
    /// </summary>
    public class NLogger
    {
        private static Logger logger;
        static NLogger()
        {
            logger = LogManager.GetCurrentClassLogger();
            logger = logger.Factory.GetLogger("FileLog");
        }
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="info">日志信息</param>
        public static void Debug(string info)
        {
            logger.Debug(info);
        }

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="info">日志信息</param>
        public static void Info(string info)
        {
            logger.Info(info);
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="info">日志信息</param>
        public static void Warn(string info)
        {
            logger.Info(info);
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="exception">异常</param>
        public static void Error(Exception exception)
        {
            logger.Error(GetExceptionFullMessage(exception));
        }

        /// <summary>
        /// 记录致命错误
        /// </summary>
        /// <param name="info">错误信息</param>
        public static void Fatal(string info)
        {
            logger.Fatal(info);
        }

        /// <summary>
        /// 获取完整的异常消息，包括内部异常消息
        /// </summary>
        /// <returns></returns>
        internal static string GetExceptionFullMessage(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }
            var message = new StringBuilder(exception.Message);
            var innerException = exception.InnerException;
            while (innerException != null)
            {
                message.Append("--->");
                message.Append(innerException.Message);
                innerException = innerException.InnerException;
            }
            return message.ToString();
        }

    }
}
