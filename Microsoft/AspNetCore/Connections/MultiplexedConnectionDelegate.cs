using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Connections
{
	internal delegate Task MultiplexedConnectionDelegate(MultiplexedConnectionContext connection);
}
