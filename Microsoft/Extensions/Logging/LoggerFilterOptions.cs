using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Extensions.Logging
{
	[DebuggerDisplay("{DebuggerToString(),nq}")]
	internal class LoggerFilterOptions
	{
		public bool CaptureScopes { get; set; } = true;


		public LogLevel MinLevel { get; set; }

		public IList<LoggerFilterRule> Rules => RulesInternal;

		internal List<LoggerFilterRule> RulesInternal { get; } = new List<LoggerFilterRule>();


		internal string DebuggerToString()
		{
			string text = ((MinLevel == LogLevel.None) ? "Enabled = false" : $"MinLevel = {MinLevel}");
			if (Rules.Count > 0)
			{
				text += $", Rules = {Rules.Count}";
			}
			return text;
		}
	}
}
