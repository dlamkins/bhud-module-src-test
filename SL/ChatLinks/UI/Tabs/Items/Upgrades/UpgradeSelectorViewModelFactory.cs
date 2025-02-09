using System.Runtime.CompilerServices;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeSelectorViewModelFactory
	{
		[CompilerGenerated]
		private IStringLocalizer<UpgradeSelector> _003Clocalizer_003EP;

		[CompilerGenerated]
		private Customizer _003Ccustomizer_003EP;

		[CompilerGenerated]
		private ItemsListViewModelFactory _003CitemsListViewModelFactory_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		public UpgradeSelectorViewModelFactory(IStringLocalizer<UpgradeSelector> localizer, Customizer customizer, ItemsListViewModelFactory itemsListViewModelFactory, IEventAggregator eventAggregator)
		{
			_003Clocalizer_003EP = localizer;
			_003Ccustomizer_003EP = customizer;
			_003CitemsListViewModelFactory_003EP = itemsListViewModelFactory;
			_003CeventAggregator_003EP = eventAggregator;
			base._002Ector();
		}

		public UpgradeSelectorViewModel Create(Item targetItem, UpgradeSlotType slotType, UpgradeComponent? selectedUpgradeComponent)
		{
			return new UpgradeSelectorViewModel(_003Clocalizer_003EP, _003Ccustomizer_003EP, _003CitemsListViewModelFactory_003EP, targetItem, slotType, selectedUpgradeComponent, _003CeventAggregator_003EP);
		}
	}
}
