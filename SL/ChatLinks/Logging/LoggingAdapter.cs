using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SL.ChatLinks.Logging
{
	public class LoggingAdapter<T> : ILogger
	{
		[CompilerGenerated]
		private string _003CcategoryName_003EP;

		[CompilerGenerated]
		private IOptionsMonitor<LoggerFilterOptions> _003Coptions_003EP;

		private readonly Logger Sink;

		public LoggingAdapter(string categoryName, IOptionsMonitor<LoggerFilterOptions> options)
		{
			_003CcategoryName_003EP = categoryName;
			_003Coptions_003EP = options;
			Sink = Logger.GetLogger<T>();
			base._002Ector();
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			Func<TState, Exception?, string> formatter2 = formatter;
			TState state2 = state;
			Exception exception2 = exception;
			if (!IsEnabled(logLevel))
			{
				return;
			}
			DateTimeOffset queued = DateTimeOffset.Now;
			LogProcessor.Enqueue(delegate
			{
				DateTimeOffset now = DateTimeOffset.Now;
				string text = $"(-{(now - queued).TotalSeconds:0.00}s) {_003CcategoryName_003EP}: {formatter2(state2, exception2)}";
				switch (logLevel)
				{
				case LogLevel.Trace:
					Sink.Trace(exception2, text);
					break;
				case LogLevel.Debug:
					Sink.Debug(exception2, text);
					break;
				case LogLevel.Information:
					Sink.Info(exception2, text);
					break;
				case LogLevel.Warning:
					Sink.Warn(exception2, text);
					break;
				case LogLevel.Error:
					Sink.Error(exception2, text);
					break;
				case LogLevel.Critical:
					Sink.Fatal(exception2, text);
					break;
				}
			});
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			foreach (LoggerFilterRule rule in _003Coptions_003EP.CurrentValue.Rules)
			{
				string providerName = rule.ProviderName;
				if ((providerName == null || providerName == "Blish_HUD") && (rule.CategoryName == null || _003CcategoryName_003EP.StartsWith(rule.CategoryName, StringComparison.OrdinalIgnoreCase)) && (!rule.LogLevel.HasValue || logLevel >= rule.LogLevel))
				{
					return rule.Filter?.Invoke("Blish_HUD", _003CcategoryName_003EP, logLevel) ?? true;
				}
			}
			return logLevel >= _003Coptions_003EP.CurrentValue.MinLevel;
		}

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		{
			return null;
		}
	}
}
