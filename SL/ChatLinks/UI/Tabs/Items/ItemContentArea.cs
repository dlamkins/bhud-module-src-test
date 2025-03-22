namespace SL.ChatLinks.UI.Tabs.Items
{
	public class ItemContentArea : ContentArea
	{
		private readonly ContentArea _previous;

		public ItemContentArea(ContentArea previous)
		{
			_previous = previous;
			base._002Ector(previous);
		}

		public override string GetTitle()
		{
			return _previous.GetTitle();
		}

		public override ContentArea Search(string text)
		{
			return _previous.Search(text);
		}
	}
}
