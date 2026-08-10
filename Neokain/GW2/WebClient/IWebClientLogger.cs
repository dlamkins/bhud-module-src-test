using System;

namespace Neokain.GW2.WebClient
{
	public interface IWebClientLogger
	{
		void LogCall(string method, object[] args);

		void LogResult(string method, object? result);

		void LogError(string method, Exception ex);

		void LogEvent(string eventName, object eventArgs);
	}
}
