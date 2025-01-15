using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildWars2.Items;

namespace SL.ChatLinks.UI.Tabs.Items.Tooltips
{
	public sealed class ItemTooltipViewModelFactory
	{
		[CompilerGenerated]
		private ItemIcons _003Cicons_003EP;

		[CompilerGenerated]
		private Customizer _003Ccustomizer_003EP;

		public ItemTooltipViewModelFactory(ItemIcons icons, Customizer customizer)
		{
			_003Cicons_003EP = icons;
			_003Ccustomizer_003EP = customizer;
			base._002Ector();
		}

		public ItemTooltipViewModel Create(Item item, int quantity, IEnumerable<UpgradeSlot> upgrades)
		{
			return new ItemTooltipViewModel(item, quantity, upgrades, _003Cicons_003EP, _003Ccustomizer_003EP);
		}
	}
}
