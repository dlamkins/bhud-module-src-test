using System.Threading.Tasks;

namespace Estreya.BlishHUD.Shared.Threading.Events
{
	public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs e);
}
