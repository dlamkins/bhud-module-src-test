using System;
using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.State
{
	public class StateConfigurations
	{
		public StateConfiguration BlishHUDAPI { get; } = new StateConfiguration
		{
			Enabled = false,
			AwaitLoading = true
		};


		public APIStateConfiguration Account { get; } = new APIStateConfiguration
		{
			Enabled = false,
			AwaitLoading = true,
			NeededPermissions = new List<TokenPermission> { (TokenPermission)1 },
			UpdateInterval = TimeSpan.FromMinutes(5.0).Add(TimeSpan.FromMilliseconds(100.0))
		};


		public APIStateConfiguration Mapchests { get; } = new APIStateConfiguration
		{
			Enabled = false,
			AwaitLoading = false,
			NeededPermissions = new List<TokenPermission>
			{
				(TokenPermission)1,
				(TokenPermission)6
			},
			UpdateInterval = TimeSpan.FromMinutes(5.0).Add(TimeSpan.FromMilliseconds(100.0))
		};


		public APIStateConfiguration Worldbosses { get; } = new APIStateConfiguration
		{
			Enabled = false,
			AwaitLoading = false,
			NeededPermissions = new List<TokenPermission>
			{
				(TokenPermission)1,
				(TokenPermission)6
			},
			UpdateInterval = TimeSpan.FromMinutes(5.0).Add(TimeSpan.FromMilliseconds(100.0))
		};


		public APIStateConfiguration PointOfInterests { get; } = new APIStateConfiguration
		{
			Enabled = false,
			AwaitLoading = false
		};


		public APIStateConfiguration Skills { get; } = new APIStateConfiguration
		{
			Enabled = false,
			AwaitLoading = false
		};


		public APIStateConfiguration TradingPost { get; } = new APIStateConfiguration
		{
			Enabled = false,
			AwaitLoading = false,
			NeededPermissions = new List<TokenPermission>
			{
				(TokenPermission)1,
				(TokenPermission)8
			},
			UpdateInterval = TimeSpan.FromMinutes(2.0)
		};


		public APIStateConfiguration Items { get; } = new APIStateConfiguration
		{
			Enabled = false,
			AwaitLoading = false
		};


		public StateConfiguration ArcDPS { get; } = new StateConfiguration
		{
			Enabled = false,
			AwaitLoading = false
		};

	}
}
