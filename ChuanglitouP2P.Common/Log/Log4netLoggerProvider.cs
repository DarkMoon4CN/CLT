using System;
using System.Configuration;
using System.Reflection;
using System.Xml;

namespace ChuanglitouP2P.Common.Log
{

    internal class Log4NetLoggerProvider : LoggerProvider
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Log4NetLoggerProvider()
        {
            var configElement = ConfigurationManager.GetSection("log4net") as XmlElement;

            if (configElement != null)
            {
                configElement.InnerXml =
                    (configElement.InnerXml.Replace("%logpath%\\", Settings.Instance.LogPath + "\\").Replace(
                        "%logpath%", Settings.Instance.LogPath + "\\"));

                log4net.Config.XmlConfigurator.Configure(configElement);
            }
        }

        public override void Info(object message)
        {
            Logger.Info(message);
        }

        public override void Error(object message)
        {
            Logger.Error( message);
        }

        public override void Error(object message, Exception exception)
        {
            Logger.Error(message, exception);
        }

        public override void Warning(object message)
        {
            Logger.Warn(message);
        }

        public override void Warning(object message, Exception exception)
        {
            Logger.Warn(message, exception);
        }
    }
}