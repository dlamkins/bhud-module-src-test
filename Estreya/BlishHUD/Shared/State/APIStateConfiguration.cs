using System;
using System.Collections.Generic;
using System.Threading;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.State
{
	public class APIStateConfiguration : StateConfiguration
	{
		public List<TokenPermission> NeededPermissions { get; set; } = new List<TokenPermission>();


		public TimeSpan UpdateInterval { get; set; } = Timeout.InfiniteTimeSpan;

	}
}
