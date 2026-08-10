using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Connections
{
	internal delegate Task ConnectionDelegate(ConnectionContext connection);
}
