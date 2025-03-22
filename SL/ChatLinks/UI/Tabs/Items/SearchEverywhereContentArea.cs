using System.Runtime.CompilerServices;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public class SearchEverywhereContentArea : ContentArea
	{
		[CompilerGenerated]
		private string _003CsearchText_003EP;

		private readonly ContentArea _previous;

		public SearchEverywhereContentArea(string searchText, ContentArea previous)
		{
			_003CsearchText_003EP = searchText;
			_previous = previous;
			base._002Ector(previous);
		}

		public override string GetTitle()
		{
			return _003CsearchText_003EP.Trim();
		}

		public override ContentArea Search(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return new RecentlyAddedContentArea();
			}
			return new SearchEverywhereContentArea(text, _previous);
		}

		public override ContentArea Back()
		{
			return _previous;
		}
	}
}
