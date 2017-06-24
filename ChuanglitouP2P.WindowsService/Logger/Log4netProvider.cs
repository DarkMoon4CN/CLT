///////////////////////////////////////////////////////////
//Name:日志模型-基于Log4Net日志执行组件封装的日志执行提供器
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Xml;
using System.Reflection;
using System.Configuration;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 基于Log4Net日志执行组件封装的日志执行提供器
    /// </summary>
    public class Log4netProvider : ILogger
    {
        private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 基于Log4Net日志执行组件封装的日志执行提供器
        /// </summary>
        /// <param name="logPath">日志存储的文件夹路径</param>
        public Log4netProvider(string logPath)
        {
            var configElement = ConfigurationManager.GetSection("log4net") as XmlElement;

            if (configElement != null)
            {
                configElement.InnerXml = (configElement.InnerXml.Replace("%logpath%\\", logPath + "\\").Replace("%logpath%", logPath + "\\"));

                log4net.Config.XmlConfigurator.Configure(configElement);
            }
        }

        public void WriteDebug(string message)
        {
            if (_Logger.IsDebugEnabled)
            { _Logger.Debug(message); }
        }

        public void WriteError(string message)
        {
            if (_Logger.IsErrorEnabled)
            { _Logger.Error(message); }
        }

        public void WriteException(string message)
        {
            if (_Logger.IsFatalEnabled)
            { _Logger.Fatal(message); }
        }

        public void WriteException(string message, Exception ex)
        {
            WriteException(message + ":" + ex.ToString());
        }

        public void WriteInfo(string message)
        {
            if (_Logger.IsInfoEnabled)
            { _Logger.Info(message); }
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
        ~Log4netProvider()
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
