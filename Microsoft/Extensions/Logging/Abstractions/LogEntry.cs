using System;

namespace Microsoft.Extensions.Logging.Abstractions
{
	internal readonly struct LogEntry<TState>
	{
		public LogLevel LogLevel { get; }

		public string Category { get; }

		public EventId EventId { get; }

		public TState State { get; }

		public Exception? Exception { get; }

		public Func<TState, Exception?, string> Formatter { get; }

		public LogEntry(LogLevel logLevel, string category, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			LogLevel = logLevel;
			Category = category;
			EventId = eventId;
			State = state;
			Exception = exception;
			Formatter = formatter;
		}
	}
}
