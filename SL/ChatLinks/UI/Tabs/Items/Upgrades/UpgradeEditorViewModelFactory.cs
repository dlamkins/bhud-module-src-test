using System.Runtime.CompilerServices;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeEditorViewModelFactory
	{
		[CompilerGenerated]
		private IStringLocalizer<UpgradeEditor> _003Clocalizer_003EP;

		[CompilerGenerated]
		private IStringLocalizer<UpgradeSlot> _003Clocalizer2_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		[CompilerGenerated]
		private IClipBoard _003Cclipboard_003EP;

		[CompilerGenerated]
		private ItemIcons _003Cicons_003EP;

		[CompilerGenerated]
		private Customizer _003Ccustomizer_003EP;

		[CompilerGenerated]
		private UpgradeSelectorViewModelFactory _003CupgradeComponentListViewModelFactory_003EP;

		[CompilerGenerated]
		private ItemTooltipViewModelFactory _003CitemTooltipViewModelFactory_003EP;

		public UpgradeEditorViewModelFactory(IStringLocalizer<UpgradeEditor> localizer, IStringLocalizer<UpgradeSlot> localizer2, IEventAggregator eventAggregator, IClipBoard clipboard, ItemIcons icons, Customizer customizer, UpgradeSelectorViewModelFactory upgradeComponentListViewModelFactory, ItemTooltipViewModelFactory itemTooltipViewModelFactory)
		{
			_003Clocalizer_003EP = localizer;
			_003Clocalizer2_003EP = localizer2;
			_003CeventAggregator_003EP = eventAggregator;
			_003Cclipboard_003EP = clipboard;
			_003Cicons_003EP = icons;
			_003Ccustomizer_003EP = customizer;
			_003CupgradeComponentListViewModelFactory_003EP = upgradeComponentListViewModelFactory;
			_003CitemTooltipViewModelFactory_003EP = itemTooltipViewModelFactory;
			base._002Ector();
		}

		public UpgradeEditorViewModel Create(Item targetItem, UpgradeSlotType slotType, UpgradeComponent? defaultUpgradeComponent)
		{
			UpgradeSlotViewModel upgradeSlotViewModel = new UpgradeSlotViewModel(slotType, _003Cicons_003EP, _003Clocalizer2_003EP, _003CitemTooltipViewModelFactory_003EP, _003CeventAggregator_003EP, _003Ccustomizer_003EP)
			{
				DefaultUpgradeComponent = defaultUpgradeComponent
			};
			return new UpgradeEditorViewModel(_003Clocalizer_003EP, _003CeventAggregator_003EP, _003Cclipboard_003EP, upgradeSlotViewModel, _003CupgradeComponentListViewModelFactory_003EP, targetItem);
		}
	}
}
