namespace Estreya.BlishHUD.EventTable.Contexts
{
	public struct RemoveEvent
	{
		public string CategoryKey { get; set; }

		public string EventKey { get; set; }

		public RemoveEvent()
		{
			CategoryKey = null;
			EventKey = null;
		}
	}
}
