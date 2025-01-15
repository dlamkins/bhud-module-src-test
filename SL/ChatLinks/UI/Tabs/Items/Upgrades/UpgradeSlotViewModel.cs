using System;
using System.Runtime.CompilerServices;
using Blish_HUD.Content;
using GuildWars2.Items;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeSlotViewModel : ViewModel
	{
		[CompilerGenerated]
		private ItemIcons _003Cicons_003EP;

		[CompilerGenerated]
		private ItemTooltipViewModelFactory _003CitemTooltipViewModelFactory_003EP;

		private UpgradeSlotType _type;

		private UpgradeComponent? _selectedUpgradeComponent;

		private UpgradeComponent? _defaultUpgradeComponent;

		public UpgradeSlotType Type
		{
			get
			{
				return _type;
			}
			set
			{
				SetField(ref _type, value, "Type");
			}
		}

		public UpgradeComponent? DefaultUpgradeComponent
		{
			get
			{
				return _defaultUpgradeComponent;
			}
			set
			{
				SetField(ref _defaultUpgradeComponent, value, "DefaultUpgradeComponent");
			}
		}

		public UpgradeComponent? SelectedUpgradeComponent
		{
			get
			{
				return _selectedUpgradeComponent;
			}
			set
			{
				SetField(ref _selectedUpgradeComponent, value, "SelectedUpgradeComponent");
			}
		}

		public UpgradeSlotViewModel(UpgradeSlotType type, ItemIcons icons, ItemTooltipViewModelFactory itemTooltipViewModelFactory)
		{
			_003Cicons_003EP = icons;
			_003CitemTooltipViewModelFactory_003EP = itemTooltipViewModelFactory;
			_type = type;
			base._002Ector();
		}

		public AsyncTexture2D? GetIcon(UpgradeComponent item)
		{
			return _003Cicons_003EP.GetIcon(item);
		}

		public ItemTooltipViewModel CreateTooltipViewModel(UpgradeComponent item)
		{
			return _003CitemTooltipViewModelFactory_003EP.Create(item, 1, Array.Empty<SL.ChatLinks.UI.Tabs.Items.Tooltips.UpgradeSlot>());
		}
	}
}
