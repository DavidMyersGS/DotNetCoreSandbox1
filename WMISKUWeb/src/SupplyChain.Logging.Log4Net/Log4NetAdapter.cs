using log4net;
using Microsoft.Extensions.Logging;
using System;

namespace GameStop.SupplyChain.Logging.log4net
{
    public class Log4NetAdapter : ILogger
    {
        private ILog _logger;

        public Log4NetAdapter(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _logger.IsDebugEnabled;
                case LogLevel.Information:
                    return _logger.IsInfoEnabled;
                case LogLevel.Warning:
                    return _logger.IsWarnEnabled;
                case LogLevel.Error:
                    return _logger.IsErrorEnabled;
                case LogLevel.Critical:
                    return _logger.IsFatalEnabled;
                default:
                    throw new ArgumentException($"Unknown log level {logLevel}.", nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                string message = null;

                if (formatter != null)
                {
                    message = formatter(state, exception);
                }

                switch (logLevel)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:
                        _logger.Debug(message, exception);
                        break;
                    case LogLevel.Information:
                        _logger.Info(message, exception);
                        break;
                    case LogLevel.Warning:
                        _logger.Warn(message, exception);
                        break;
                    case LogLevel.Error:
                        _logger.Error(message, exception);
                        break;
                    case LogLevel.Critical:
                        _logger.Fatal(message, exception);
                        break;
                    default:
                        _logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _logger.Info(message, exception);
                        break;
                }
            }
            else
            {
                return;
            }
        }
    }
}