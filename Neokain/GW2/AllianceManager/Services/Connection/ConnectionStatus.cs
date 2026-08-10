using System;

namespace Neokain.GW2.AllianceManager.Services.Connection
{
	public sealed class ConnectionStatus
	{
		public ModuleConnectionState State { get; set; }

		public string Reason { get; set; } = string.Empty;


		public string Endpoint { get; set; } = string.Empty;


		public string AccountName { get; set; }

		public DateTime? NextRetryUtc { get; set; }

		public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

	}
}
