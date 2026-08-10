using System;
using Neokain.GW2.WebClient.Models.Connection;

namespace Neokain.GW2.WebClient
{
	public sealed class CompatibilityFailedEventArgs : EventArgs
	{
		public string ServerVersion { get; }

		public string? LatestClientVersion { get; }

		public string CompatibilityMessage { get; }

		public string? DownloadUrl { get; }

		public CompatibilityFailedEventArgs(ServerHandshakeResponseDto response)
		{
			ServerVersion = response.ServerVersion;
			LatestClientVersion = response.LatestClientVersion;
			CompatibilityMessage = response.CompatibilityMessage ?? "Your version is not compatible with the server. Please update!";
			DownloadUrl = response.DownloadUrl;
		}
	}
}
