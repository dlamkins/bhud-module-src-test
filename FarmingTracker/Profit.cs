namespace FarmingTracker
{
	public class Profit
	{
		private long? _unsigned_Custom_ProfitInCopper;

		private readonly object _lock = new object();

		public bool CanNotBeSold { get; set; } = true;


		public bool CanBeSoldOnTp { get; set; }

		public bool CanBeSoldToVendor { get; set; }

		public bool HasCustomProfit => Unsigned_Custom_ProfitInCopper.HasValue;

		public long Unsigned_Max_ProfitInCopper => Unsigned_Custom_ProfitInCopper ?? Unsigned_MaxTpAndVendor_ProfitInCopper.Value;

		public long? Unsigned_Custom_ProfitInCopper
		{
			get
			{
				lock (_lock)
				{
					return _unsigned_Custom_ProfitInCopper;
				}
			}
			set
			{
				lock (_lock)
				{
					_unsigned_Custom_ProfitInCopper = value;
				}
			}
		}

		public ThreadSafeLong Unsigned_MaxTpAndVendor_ProfitInCopper { get; } = new ThreadSafeLong();


		public ThreadSafeLong Unsigned_MaxTp_ProfitInCopper { get; } = new ThreadSafeLong();


		public ThreadSafeLong Unsigned_TpSell_ProfitInCopper { get; } = new ThreadSafeLong();


		public ThreadSafeLong Unsigned_TpBuy_ProfitInCopper { get; } = new ThreadSafeLong();


		public ThreadSafeLong Unsigned_Vendor_ProfitInCopper { get; } = new ThreadSafeLong();

	}
}
