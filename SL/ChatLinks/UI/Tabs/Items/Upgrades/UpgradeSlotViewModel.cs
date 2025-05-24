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
		public delegate UpgradeSlotViewModel Factory(UpgradeSlotType type, UpgradeComponent? defaultUpgradeComponent);

		private UpgradeSlotType _type;

		private UpgradeComponent? _selectedUpgradeComponent;

		private UpgradeComponent? _defaultUpgradeComponent;

		private readonly IconsService _icons;

		private readonly IStringLocalizer<UpgradeSlot> _localizer;

		private readonly ItemTooltipViewModel.Factory _itemTooltipViewModelFactory;

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

		public UpgradeSlotViewModel(IconsService icons, IStringLocalizer<UpgradeSlot> localizer, ItemTooltipViewModel.Factory itemTooltipViewModelFactory, IEventAggregator eventAggregator, Customizer customizer, UpgradeSlotType type, UpgradeComponent? defaultUpgradeComponent)
		{
			ThrowHelper.ThrowIfNull(eventAggregator, "eventAggregator");
			_icons = icons;
			_localizer = localizer;
			_itemTooltipViewModelFactory = itemTooltipViewModelFactory;
			_eventAggregator = eventAggregator;
			_customizer = customizer;
			_type = type;
			_defaultUpgradeComponent = defaultUpgradeComponent;
			eventAggregator.Subscribe(new Func<LocaleChanged, Task>(OnLocaleChanged));
		}

		private async Task OnLocaleChanged(LocaleChanged changed)
		{
			OnPropertyChanged("EmptySlotTooltip");
			if ((object)SelectedUpgradeComponent != null)
			{
				int id2 = SelectedUpgradeComponent!.Id;
				SelectedUpgradeComponent = await _customizer.GetUpgradeComponentAsync(id2).ConfigureAwait(continueOnCapturedContext: false);
			}
			if ((object)DefaultUpgradeComponent != null)
			{
				int id = DefaultUpgradeComponent!.Id;
				DefaultUpgradeComponent = await _customizer.GetUpgradeComponentAsync(id).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				OnPropertyChanged("DefaultUpgradeComponent");
			}
		}

		public AsyncTexture2D? GetIcon(UpgradeComponent item)
		{
			ThrowHelper.ThrowIfNull(item, "item");
			return _icons.GetIcon(item.IconUrl);
		}

		public ItemTooltipViewModel CreateTooltipViewModel(UpgradeComponent item)
		{
			return _itemTooltipViewModelFactory(item, 1, Array.Empty<SL.ChatLinks.UI.Tabs.Items.Tooltips.UpgradeSlot>());
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<LocaleChanged>(new Func<LocaleChanged, Task>(OnLocaleChanged));
		}
	}
}
