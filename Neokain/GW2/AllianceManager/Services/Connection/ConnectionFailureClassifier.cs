using System;
using Neokain.GW2.WebClient;

namespace Neokain.GW2.AllianceManager.Services.Connection
{
	public static class ConnectionFailureClassifier
	{
		public static ConnectionFailure Classify(Exception ex)
		{
			if (ex is IncompatibleVersionException)
			{
				return new ConnectionFailure(FailureKind.Terminal, "Module version is incompatible with the server.");
			}
			string msg = ex?.Message ?? string.Empty;
			if (msg.IndexOf("401", StringComparison.OrdinalIgnoreCase) >= 0 || msg.IndexOf("Unauthorized", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return new ConnectionFailure(FailureKind.Terminal, "API key was rejected by the server (401).");
			}
			return new ConnectionFailure(FailureKind.Transient, "Could not reach the server. Retrying…");
		}
	}
}
