using System;
using System.Threading;

namespace Estreya.BlishHUD.Shared.State
{
	public class StateConfiguration
	{
		public bool Enabled { get; set; }

		public bool AwaitLoading { get; set; } = true;


		public TimeSpan SaveInterval { get; set; } = Timeout.InfiniteTimeSpan;

	}
}
