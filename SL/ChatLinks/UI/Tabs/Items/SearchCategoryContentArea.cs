using System.Runtime.CompilerServices;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public class SearchCategoryContentArea : ContentArea
	{
		[CompilerGenerated]
		private string _003Clabel_003EP;

		[CompilerGenerated]
		private string _003CsearchText_003EP;

		public SearchCategoryContentArea(string label, string searchText, ContentArea previous)
		{
			_003Clabel_003EP = label;
			_003CsearchText_003EP = searchText;
			base._002Ector(previous);
		}

		public override string GetTitle()
		{
			return _003Clabel_003EP + "—" + _003CsearchText_003EP.Trim();
		}

		public override ContentArea Search(string text)
		{
			ThrowHelper.ThrowIfNull(base.Previous, "Previous");
			if (!string.IsNullOrWhiteSpace(text))
			{
				return new SearchCategoryContentArea(_003Clabel_003EP, text, base.Previous);
			}
			return base.Previous;
		}
	}
}
