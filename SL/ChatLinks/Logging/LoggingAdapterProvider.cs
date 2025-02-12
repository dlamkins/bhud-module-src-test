using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SL.ChatLinks.Logging
{
	public class LoggingAdapterProvider<T> : ILoggerProvider, IDisposable
	{
		[CompilerGenerated]
		private IOptionsMonitor<LoggerFilterOptions> _003Coptions_003EP;

		public LoggingAdapterProvider(IOptionsMonitor<LoggerFilterOptions> options)
		{
			_003Coptions_003EP = options;
			base._002Ector();
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new LoggingAdapter<T>(categoryName, _003Coptions_003EP);
		}

		public void Dispose()
		{
		}
	}
}
