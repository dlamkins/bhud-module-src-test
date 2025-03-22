using System.Runtime.CompilerServices;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public class CategoryContentArea : ContentArea
	{
		[CompilerGenerated]
		private string _003Clabel_003EP;

		public CategoryContentArea(string label)
		{
			_003Clabel_003EP = label;
			base._002Ector(new RecentlyAddedContentArea());
		}

		public override string GetTitle()
		{
			return _003Clabel_003EP;
		}

		public override ContentArea Search(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return this;
			}
			return new SearchCategoryContentArea(_003Clabel_003EP, text, this);
		}
	}
}
