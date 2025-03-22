using System.Runtime.CompilerServices;
using Blish_HUD.Content;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public abstract class ContentArea
	{
		[CompilerGenerated]
		private ContentArea? _003Cprevious_003EP;

		protected ContentArea(ContentArea? previous = null)
		{
			_003Cprevious_003EP = previous;
			base._002Ector();
		}

		public abstract string GetTitle();

		public virtual AsyncTexture2D? GetIcon()
		{
			if (_003Cprevious_003EP == null)
			{
				return null;
			}
			return AsyncTexture2D.FromAssetId(784268);
		}

		public abstract ContentArea Search(string text);

		public virtual ContentArea Back()
		{
			return _003Cprevious_003EP ?? this;
		}

		public virtual ItemContentArea SelectItem()
		{
			return new ItemContentArea(this);
		}
	}
}
