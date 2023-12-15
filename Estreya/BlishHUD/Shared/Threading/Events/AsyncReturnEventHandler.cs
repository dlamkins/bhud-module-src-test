using System.Threading.Tasks;

namespace Estreya.BlishHUD.Shared.Threading.Events
{
	public delegate Task<TReturn> AsyncReturnEventHandler<TReturn>(object sender);
	public delegate Task<TReturn> AsyncReturnEventHandler<TEventArgs, TReturn>(object sender, TEventArgs e);
}
