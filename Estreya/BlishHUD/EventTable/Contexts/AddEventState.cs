using System;
using Estreya.BlishHUD.EventTable.Services;

namespace Estreya.BlishHUD.EventTable.Contexts
{
	public struct AddEventState
	{
		public string AreaName;

		public string EventKey;

		public EventStateService.EventStates State;

		public DateTime Until;

		public AddEventState()
		{
			AreaName = null;
			EventKey = null;
			State = EventStateService.EventStates.Hidden;
			Until = DateTime.UtcNow;
		}
	}
}
