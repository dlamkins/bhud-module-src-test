using Estreya.BlishHUD.EventTable.Models;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class EventDetailsButton : DataDetailsButton<Estreya.BlishHUD.EventTable.Models.Event>
	{
		public Estreya.BlishHUD.EventTable.Models.Event Event
		{
			get
			{
				return base.Data;
			}
			set
			{
				base.Data = value;
			}
		}
	}
}
