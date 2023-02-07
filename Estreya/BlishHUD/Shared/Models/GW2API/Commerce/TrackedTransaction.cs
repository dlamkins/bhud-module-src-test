using Humanizer;

namespace Estreya.BlishHUD.Shared.Models.GW2API.Commerce
{
	public class TrackedTransaction : Transaction
	{
		public int WishPrice { get; set; }

		public override string ToString()
		{
			return $"Item-ID: {base.ItemId} - Type: {base.Type.Humanize()} - Quantity: {base.Quantity} - Unit Price: {base.Price} - Wish Price: {WishPrice}";
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() * 23 + WishPrice.GetHashCode();
		}
	}
}
