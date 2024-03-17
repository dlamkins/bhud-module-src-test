using System;
using System.Threading;

namespace Estreya.BlishHUD.Shared.Services
{
	public class ServiceConfiguration
	{
		public bool Enabled { get; set; }

		public bool AwaitLoading { get; set; } = true;


		public TimeSpan SaveInterval { get; set; } = Timeout.InfiniteTimeSpan;

	}
}
