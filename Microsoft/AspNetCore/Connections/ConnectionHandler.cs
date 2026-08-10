using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Connections
{
	internal abstract class ConnectionHandler
	{
		public abstract Task OnConnectedAsync(ConnectionContext connection);
	}
}
