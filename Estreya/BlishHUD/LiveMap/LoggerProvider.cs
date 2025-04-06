using System;
using Microsoft.Extensions.Logging;

namespace Estreya.BlishHUD.LiveMap
{
	internal class LoggerProvider : ILoggerProvider, IDisposable
	{
		public ILogger CreateLogger(string categoryName)
		{
			return new CustomLogger();
		}

		public void Dispose()
		{
		}
	}
}
