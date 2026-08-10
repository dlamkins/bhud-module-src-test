using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Microsoft.AspNetCore.SignalR
{
	internal class JsonHubProtocolOptions
	{
		public JsonSerializerOptions PayloadSerializerOptions { get; set; } = JsonHubProtocol.CreateDefaultSerializerSettings();

	}
}
