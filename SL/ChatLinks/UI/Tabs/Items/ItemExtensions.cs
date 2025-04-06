using System;
using GuildWars2.Items;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	internal static class ItemExtensions
	{
		public static Uri? IconUrl(this Item item)
		{
			ThrowHelper.ThrowIfNull(item, "item");
			if (string.IsNullOrEmpty(item.IconHref))
			{
				return null;
			}
			return new Uri(item.IconHref);
		}

		public static Uri? IconUrl(this Effect effect)
		{
			ThrowHelper.ThrowIfNull(effect, "effect");
			if (string.IsNullOrEmpty(effect.IconHref))
			{
				return null;
			}
			return new Uri(effect.IconHref);
		}
	}
}
