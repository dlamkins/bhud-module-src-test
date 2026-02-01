using Blish_HUD.Controls;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class DataFlowPanel<T> : FlowPanel
	{
		public T Data { get; set; }

		public DataFlowPanel(T data)
			: this()
		{
			Data = data;
		}
	}
}
