using log4net;
using log4net.Config;
using System;
using System.Reflection;

namespace ConnectorAccess
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class Logger
    {
        #region Members

        private readonly ILog _logger = LogManager.GetLogger(typeof(Logger));

        #endregion

        #region Constructors

        public Logger()
        {
            XmlConfigurator.Configure();
        }

        #endregion

        #region Methods

        public void Debug(String log, Exception ex)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Info(log, ex);
            }
        }

        public void Debug(String log)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Info(log);
            }
        }

        public void Error(String log, Exception ex)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.Error(log, ex);
            }
        }

        public void Error(String log)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.Error(log);
            }
        }

        public void Fatal(String log, Exception ex)
        {
            if (_logger.IsFatalEnabled)
            {
                _logger.Fatal(log, ex);
            }
        }

        public void Info(String log)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(log);
            }
        }

        public void Warn(String log, Exception ex)
        {
            if (_logger.IsWarnEnabled)
            {
                _logger.Warn(log, ex);
            }
        }

        #endregion
    }
}
