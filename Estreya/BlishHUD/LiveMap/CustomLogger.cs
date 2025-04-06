using System;
using Blish_HUD;
using Microsoft.Extensions.Logging;

namespace Estreya.BlishHUD.LiveMap
{
	internal class CustomLogger : ILogger
	{
		private Logger _logger = Logger.GetLogger(typeof(CustomLogger));

		public IDisposable BeginScope<TState>(TState state) where TState : notnull
		{
			return null;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			string message = formatter(state, exception);
			switch (logLevel)
			{
			case LogLevel.Error:
			case LogLevel.Critical:
				_logger.Error(message);
				break;
			case LogLevel.Warning:
				_logger.Warn(message);
				break;
			case LogLevel.Information:
				_logger.Info(message);
				break;
			case LogLevel.Trace:
			case LogLevel.Debug:
				_logger.Debug(message);
				break;
			default:
				_logger.Info(message);
				break;
			}
		}
	}
}
