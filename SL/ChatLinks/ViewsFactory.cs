using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Graphics.UI;
using GuildWars2.Items;
using Microsoft.Extensions.DependencyInjection;
using SL.ChatLinks.UI;
using SL.ChatLinks.UI.Tabs.Items;
using SL.Common.Controls.Items;

namespace SL.ChatLinks
{
	public sealed class ViewsFactory : IViewsFactory
	{
		[CompilerGenerated]
		private IServiceProvider _003CserviceProvider_003EP;

		public ViewsFactory(IServiceProvider serviceProvider)
		{
			_003CserviceProvider_003EP = serviceProvider;
			base._002Ector();
		}

		public IView CreateItemsTabView()
		{
			return (IView)(object)new AsyncView(() => (IView)(object)ActivatorUtilities.CreateInstance<ItemsTabView>(_003CserviceProvider_003EP, Array.Empty<object>()));
		}

		public ITooltipView CreateItemTooltipView(Item item, IReadOnlyDictionary<int, UpgradeComponent> upgrades)
		{
			return (ITooltipView)(object)ActivatorUtilities.CreateInstance<ItemTooltipView>(_003CserviceProvider_003EP, new object[2] { item, upgrades });
		}
	}
}
