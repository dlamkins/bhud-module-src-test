namespace FarmingTracker
{
	public class Profits
	{
		public bool CanNotBeSold { get; set; } = true;


		public bool CanBeSoldOnTp { get; set; }

		public bool CanBeSoldToVendor { get; set; }

		public Profit Each { get; set; } = new Profit();


		public Profit All { get; set; } = new Profit();

	}
}
