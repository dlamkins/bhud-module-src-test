using System;
using System.Threading.Tasks;
using Blish_HUD.Content;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeSlotViewModel : ViewModel, IDisposable
	{
		private UpgradeSlotType _type;

		private UpgradeComponent? _selectedUpgradeComponent;

		private UpgradeComponent? _defaultUpgradeComponent;

		private readonly ItemIcons _icons;

		private readonly IStringLocalizer<UpgradeSlot> _localizer;

		private readonly ItemTooltipViewModelFactory _itemTooltipViewModelFactory;

		private readonly IEventAggregator _eventAggregator;

		private readonly Customizer _customizer;

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

		public string EmptySlotTooltip => (string)_localizer["Empty slot tooltip"];

		public string UnusedUpgradeSlotLabel => (string)_localizer["Unused upgrade slot"];

		public string UnusedInfusionSlotLabel => (string)_localizer["Unused infusion slot"];

		public string UnusedEnrichmenSlotLabel => (string)_localizer["Unused enrichment slot"];

		public UpgradeSlotViewModel(UpgradeSlotType type, ItemIcons icons, IStringLocalizer<UpgradeSlot> localizer, ItemTooltipViewModelFactory itemTooltipViewModelFactory, IEventAggregator eventAggregator, Customizer customizer)
		{
			_icons = icons;
			_localizer = localizer;
			_itemTooltipViewModelFactory = itemTooltipViewModelFactory;
			_eventAggregator = eventAggregator;
			_customizer = customizer;
			_type = type;
			eventAggregator.Subscribe(new Func<LocaleChanged, ValueTask>(OnLocaleChanged));
		}

		private async ValueTask OnLocaleChanged(LocaleChanged changed)
		{
			OnPropertyChanged("EmptySlotTooltip");
			if ((object)SelectedUpgradeComponent != null)
			{
				int id2 = SelectedUpgradeComponent!.Id;
				SelectedUpgradeComponent = await _customizer.GetUpgradeComponentAsync(id2);
			}
			if ((object)DefaultUpgradeComponent != null)
			{
				int id = DefaultUpgradeComponent!.Id;
				DefaultUpgradeComponent = await _customizer.GetUpgradeComponentAsync(id);
			}
			else
			{
				OnPropertyChanged("DefaultUpgradeComponent");
			}
		}

		public AsyncTexture2D? GetIcon(UpgradeComponent item)
		{
			return _icons.GetIcon(item);
		}

		public ItemTooltipViewModel CreateTooltipViewModel(UpgradeComponent item)
		{
			return _itemTooltipViewModelFactory.Create(item, 1, Array.Empty<SL.ChatLinks.UI.Tabs.Items.Tooltips.UpgradeSlot>());
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<LocaleChanged>(new Func<LocaleChanged, ValueTask>(OnLocaleChanged));
		}
	}
}
