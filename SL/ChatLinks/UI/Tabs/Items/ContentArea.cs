using Blish_HUD.Content;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public abstract class ContentArea
	{
		protected ContentArea? Previous { get; }

		protected ContentArea(ContentArea? previous = null)
		{
			Previous = previous;
			base._002Ector();
		}

		public abstract string GetTitle();

		public virtual AsyncTexture2D? GetIcon()
		{
			if (Previous == null)
			{
				return null;
			}
			return AsyncTexture2D.FromAssetId(784268);
		}

		public abstract ContentArea Search(string text);

		public virtual ContentArea Back()
		{
			return Previous ?? this;
		}

		public virtual ItemContentArea SelectItem()
		{
			return new ItemContentArea(this);
		}
	}
}
