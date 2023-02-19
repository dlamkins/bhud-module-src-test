using Blish_HUD.Controls;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class DataDetailsButton<T> : DetailsButton
	{
		public T Data { get; set; }

		public DataDetailsButton()
			: this()
		{
		}
	}
}
