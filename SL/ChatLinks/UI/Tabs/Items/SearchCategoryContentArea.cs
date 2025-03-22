using System.Runtime.CompilerServices;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public class SearchCategoryContentArea : ContentArea
	{
		[CompilerGenerated]
		private string _003Clabel_003EP;

		[CompilerGenerated]
		private string _003CsearchText_003EP;

		[CompilerGenerated]
		private ContentArea _003Cprevious_003EP;

		public SearchCategoryContentArea(string label, string searchText, ContentArea previous)
		{
			_003Clabel_003EP = label;
			_003CsearchText_003EP = searchText;
			_003Cprevious_003EP = previous;
			base._002Ector(_003Cprevious_003EP);
		}

		public override string GetTitle()
		{
			return _003Clabel_003EP + "—" + _003CsearchText_003EP.Trim();
		}

		public override ContentArea Search(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return _003Cprevious_003EP;
			}
			return new SearchCategoryContentArea(_003Clabel_003EP, text, _003Cprevious_003EP);
		}

		public override ContentArea Back()
		{
			return _003Cprevious_003EP;
		}
	}
}
