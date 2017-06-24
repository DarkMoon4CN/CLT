using System;
using System.Configuration.Provider;

namespace ChuanglitouP2P.Common.Log
{
    public abstract class LoggerProvider : ProviderBase
    {
        private static LoggerProvider _instance = new Log4NetLoggerProvider();

        public static LoggerProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Log4NetLoggerProvider();
                return _instance;
            }
        }

        public abstract void Info(object message);

        public abstract void Error(object message);

        public abstract void Error(object message, Exception exception);

        public abstract void Warning(object message);

        public abstract void Warning(object message, Exception exception);
    }
}