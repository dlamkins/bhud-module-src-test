using System.Collections.Generic;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class EventChangedArgs
	{
		public bool OldState { get; set; }

		public bool NewState { get; set; }

		public Dictionary<string, object> AdditionalData { get; set; }

		public string EventSettingKey { get; set; }
	}
}
