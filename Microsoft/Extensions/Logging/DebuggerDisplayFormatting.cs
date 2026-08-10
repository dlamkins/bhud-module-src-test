using System;

namespace Microsoft.Extensions.Logging
{
	internal static class DebuggerDisplayFormatting
	{
		internal static string DebuggerToString(string name, ILogger logger)
		{
			LogLevel? logLevel = CalculateEnabledLogLevel(logger);
			string text = "Name = \"" + name + "\"";
			if (logLevel.HasValue)
			{
				return text + $", MinLevel = {logLevel}";
			}
			return text + ", Enabled = false";
		}

		internal static LogLevel? CalculateEnabledLogLevel(ILogger logger)
		{
			Span<LogLevel> span = stackalloc LogLevel[6]
			{
				LogLevel.Critical,
				LogLevel.Error,
				LogLevel.Warning,
				LogLevel.Information,
				LogLevel.Debug,
				LogLevel.Trace
			};
			ReadOnlySpan<LogLevel> readOnlySpan = span;
			LogLevel? result = null;
			ReadOnlySpan<LogLevel> readOnlySpan2 = readOnlySpan;
			for (int i = 0; i < readOnlySpan2.Length; i++)
			{
				LogLevel logLevel = readOnlySpan2[i];
				if (!logger.IsEnabled(logLevel))
				{
					break;
				}
				result = logLevel;
			}
			return result;
		}
	}
}
