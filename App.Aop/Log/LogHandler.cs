using System.Collections.Generic;
using System.IO;
using NLog;
using Newtonsoft.Json.Linq;

namespace App.Aop.Log
{
    /// <summary>
    /// 通用日志记录
    /// </summary>
    /// <typeparam name="TLog">日志对象</typeparam>
    public class LogHandler<TLog>
    {
        private Logger logger;
        private static LogFactory factory;
        // public static ILoggerRepository _repository = LogManager.CreateRepository("NETCoreRepository");
        protected LogHandler(LogMode logMode)
        {
            LoggerMode = logMode;
            if (factory == null)
            {
                //加载配置
                factory = LogManager.LoadConfiguration(Directory.GetCurrentDirectory() + "/Configs/nlog.config");
            }
            logger = factory.GetLogger(LoggerMode.ToString());
        }

        /// <summary>
        /// 日志对象实体
        /// </summary>
        public TLog LogInfo { get; set; }

        /// <summary>
        /// 记录日志模式
        /// </summary>
        private LogMode LoggerMode { get; set; }

        /// <summary>
        /// 写入日志记录
        /// </summary>
        public virtual void WriteLog()
        {
            var dic = JObject.FromObject(LogInfo).ToObject<Dictionary<string, string>>();
            LogEventInfo logEventInfo = new LogEventInfo(LogLevel.Debug, LoggerMode.ToString(), "");
            foreach (var item in dic)
            {
                logEventInfo.Properties[item.Key] = item.Value;
            }
            logger.Log(logEventInfo);
        }
    }

    /// <summary>
    /// 记录日志类别
    /// </summary>
    public enum LogMode
    {
        /// <summary>
        /// 异常日志
        /// </summary>
        ExceptionLog,
        /// <summary>
        /// 登录日志
        /// </summary>
        LoginLog,
        /// <summary>
        /// 操作日志
        /// </summary>
        OperateLog,
        /// <summary>
        /// SQL日志
        /// </summary>
        SqlLog
    }
}
