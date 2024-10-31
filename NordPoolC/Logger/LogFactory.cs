using CaoNC.Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Logger
{
    public class LogFactory
    {
        private static Lazy<LogFactory> logFactory = new Lazy<LogFactory>();
        public static LogFactory Instance => logFactory.Value;

        private Action<string> LogDebug;
        private Action<string> LogWarning;
        private Action<string> LogInfo;
        private Action<string> LogError;
        private Action<Exception,string> LogException;

        /// <summary>
        /// 注册日志debug
        /// </summary>
        /// <param name="logDebug">日志debug动作</param>
        /// <returns>日志factory</returns>
        public LogFactory RegisterDebugLog(Action<string> logDebug)
        {
            LogDebug = logDebug;
            return this;
        }

        /// <summary>
        /// 注册日志warn
        /// </summary>
        /// <param name="logDebug">日志warn动作</param>
        /// <returns>日志factory</returns>
        public LogFactory RegisterWarningLog(Action<string> logWarning)
        {
            LogWarning = logWarning;
            return this;
        }

        /// <summary>
        /// 注册日志info
        /// </summary>
        /// <param name="logDebug">日志info动作</param>
        /// <returns>日志factory</returns>
        public LogFactory RegisterInfoLog(Action<string> logInfo)
        {
            LogInfo = logInfo;
            return this;
        }

        /// <summary>
        /// 注册日志error
        /// </summary>
        /// <param name="logDebug">日志error动作</param>
        /// <returns>日志factory</returns>
        public LogFactory RegisterErrorLog(Action<string> logError)
        {
            LogError = logError;
            return this;
        }

        /// <summary>
        /// 注册日志exception
        /// </summary>
        /// <param name="logDebug">日志注册日志exception动作</param>
        /// <returns>日志factory</returns>
        public LogFactory RegisterExceptionLog(Action<Exception, string> logException)
        {
            LogException = logException;
            return this;
        }

        public void Debug(string msg)
        {
            LogDebug?.Invoke(msg);
        }

        public void Warning(string msg)
        {
            LogWarning?.Invoke(msg);
        }

        public void Info(string msg)
        {
            LogInfo?.Invoke(msg);
        }

        public void Error(string msg)
        {
            LogError?.Invoke(msg);
        }

        public void Exception(Exception e,string msg)
        {
            LogException?.Invoke(e,msg);
        }

        public void DebugAsync(string msg)
        {
            LogDebug?.BeginInvoke(msg, null, null);
        }

        public void WarningAsync(string msg)
        {
            LogWarning?.BeginInvoke(msg, null, null);
        }

        public void InfoAsync(string msg)
        {
            LogInfo?.BeginInvoke(msg, null, null);
        }

        public void ErrorAsync(string msg)
        {
            LogError?.BeginInvoke(msg, null, null);
        }

        public void ExceptionAsync(Exception e, string msg)
        {
            LogException?.BeginInvoke(e, msg,null,null);
        }
    }
}
