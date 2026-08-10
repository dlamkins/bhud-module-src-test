using System;
using System.Collections;
using Blish_HUD;
using Neokain.GW2.WebClient;

namespace Neokain.GW2.AllianceManager.Services.Logging
{
	internal sealed class WebClientLoggerAdapter : IWebClientLogger
	{
		private static readonly Logger Log = Logger.GetLogger<WebClientLoggerAdapter>();

		public void LogCall(string method, object[] args)
		{
			Log.Debug("[Hub] Call: " + method + "(" + FormatArgs(args) + ")");
		}

		public void LogResult(string method, object result)
		{
			Log.Debug("[Hub] Result: " + method + " returned " + FormatResult(result));
		}

		public void LogError(string method, Exception ex)
		{
			Log.Error(ex, "[Hub] Error: " + method);
		}

		public void LogEvent(string eventName, object eventArgs)
		{
			Log.Debug("[Hub] Event: " + eventName + " - " + FormatEventArgs(eventArgs));
		}

		private static string FormatArgs(object[] args)
		{
			if (args == null || args.Length == 0)
			{
				return "";
			}
			string[] parts = new string[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				parts[i] = FormatValue(args[i]);
			}
			return string.Join(", ", parts);
		}

		private static string FormatResult(object result)
		{
			if (result == null)
			{
				return "null";
			}
			ICollection collection = result as ICollection;
			if (collection != null)
			{
				return $"({collection.Count} items)";
			}
			return result.ToString();
		}

		private static string FormatEventArgs(object eventArgs)
		{
			if (eventArgs == null)
			{
				return "(no args)";
			}
			return eventArgs.ToString();
		}

		private static string FormatValue(object value)
		{
			if (value == null)
			{
				return "null";
			}
			if (value is Guid)
			{
				return ((Guid)value).ToString().Substring(0, 8) + "...";
			}
			string str = value as string;
			if (str != null)
			{
				if (str.Length <= 50)
				{
					return str;
				}
				return str.Substring(0, 47) + "...";
			}
			return value.ToString();
		}
	}
}
