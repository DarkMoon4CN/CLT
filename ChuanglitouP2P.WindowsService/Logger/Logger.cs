///////////////////////////////////////////////////////////
//Name:日志模型-日志对象默认实体类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Diagnostics;

namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 日志对象默认实体类
    /// </summary>
    public class Logger : ILogger, IDisposable
    {
        private ILogger _instance = null;

        /// <summary>
        /// 日志对象默认实体类
        /// </summary>
        protected Logger() { }
        /// <summary>
        /// 日志对象默认实体类
        /// </summary>
        /// <param name="logger">日志记录执行提供器</param>
        public Logger(ILogger logger)
        {
            _instance = logger;
        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public void WriteError(string message)
        {
            _instance.WriteError(LogFormat(LogTypeEnum.Error, message));
        }

        /// <summary>
        /// 记录运行时信息日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public void WriteInfo(string message)
        {
            _instance.WriteInfo(LogFormat(LogTypeEnum.Info, message));
        }

        /// <summary>
        /// 记录运行时调试日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public void WriteDebug(string message)
        {
            _instance.WriteDebug(message);
        }

        /// <summary>
        /// 记录运行时异常日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public void WriteException(string message)
        {
            _instance.WriteException(LogFormat(LogTypeEnum.Exception, message));
        }

        /// <summary>
        /// 记录运行时异常日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="ex">运行时截获的异常实体对象</param>
        public void WriteException(string message, Exception ex) { _instance.WriteException(LogFormat(LogTypeEnum.Exception, message, ex)); }

        protected virtual string LogFormat(LogTypeEnum type, string message, Exception ex = null)
        {
            var caller = GetCallerInfo();
            string logTemplate = "-------------------------------------------------------------------------------" + Enum.GetName(typeof(LogTypeEnum), type);
            string logType = string.Empty;
            string contentPrefix = ">>> ";
            switch (type)
            {
                case LogTypeEnum.Debug: logType = "调试信息"; break;
                case LogTypeEnum.Error: logType = "错误信息"; break;
                case LogTypeEnum.Exception: logType = "异常信息"; break;
                case LogTypeEnum.Info: logType = "追踪信息"; break;
            }
            logTemplate += "记录时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff") + Environment.NewLine;
            logTemplate += "记录类型: " + logType + Environment.NewLine;
            logTemplate += "记录来源: " + Environment.NewLine;
            logTemplate += contentPrefix + "FilePath: " + caller.FileName + " ClassName: " + caller.ClassName + " MethodName:" + caller.MethodName + " Line: " + caller.LineNumber.ToString() + " Column: " + caller.ColumnNumber.ToString() + Environment.NewLine;
            logTemplate += "记录内容: " + Environment.NewLine;
            logTemplate += contentPrefix + message + Environment.NewLine;
            if (ex != null)
            {
                logTemplate += "记录异常: " + Environment.NewLine;
                logTemplate += contentPrefix + ex.ToString() + Environment.NewLine;
            }
            return logTemplate;
        }

        protected virtual CallerModel GetCallerInfo()
        {
            StackFrame stackFrame = new StackFrame(3);
            string fileName = stackFrame.GetFileName();
            string className = stackFrame.GetMethod().Module.FullyQualifiedName;
            return new CallerModel()
            {
                ClassName = className,
                FileName = fileName,
                MethodName = stackFrame.GetMethod().Name,
                LineNumber = stackFrame.GetFileLineNumber(),
                ColumnNumber = stackFrame.GetFileColumnNumber()
            };
        }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.Collect();
            }
            else
            {
                GC.SuppressFinalize(this);
            }
        }
        ~Logger()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
