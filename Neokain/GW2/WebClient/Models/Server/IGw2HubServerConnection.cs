using System.Threading.Tasks;
using Neokain.GW2.WebClient.Models.Connection;

namespace Neokain.GW2.WebClient.Models.Server
{
	internal interface IGw2HubServerConnection
	{
		Task<ServerHandshakeResponseDto> Handshake(ClientHandshakeDto request);
	}
}
