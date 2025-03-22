namespace SL.ChatLinks.UI.Tabs.Items
{
	public class RecentlyAddedContentArea : ContentArea
	{
		public override string GetTitle()
		{
			return " ";
		}

		public override ContentArea Search(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return this;
			}
			return new SearchEverywhereContentArea(text, this);
		}
	}
}
