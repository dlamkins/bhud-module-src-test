using System;

namespace BhModule.Community.Pathing.Scripting
{
	public struct ScriptMessage
	{
		public DateTime Timestamp { get; }

		public string Message { get; }

		public string Source { get; }

		public ScriptMessageLogLevel LogLevel { get; }

		public ScriptMessage(string message, string source, DateTime timestamp, ScriptMessageLogLevel logLevel = ScriptMessageLogLevel.Info)
		{
			Message = message;
			Source = source;
			Timestamp = timestamp;
			LogLevel = logLevel;
		}
	}
}
