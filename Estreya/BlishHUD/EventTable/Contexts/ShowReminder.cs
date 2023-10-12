namespace Estreya.BlishHUD.EventTable.Contexts
{
	public struct ShowReminder
	{
		public string Title { get; set; }

		public string Message { get; set; }

		public string Icon { get; set; }

		public ShowReminder()
		{
			Title = null;
			Message = null;
			Icon = null;
		}
	}
}
