using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Blish_HUD.Content;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Collections
{
	public sealed class ItemsListViewModel : ViewModel, IDisposable
	{
		private readonly IStringLocalizer<ItemsList> _localizer;

		private readonly IEventAggregator _eventAggregator;

		private readonly IClipBoard _clipboard;

		private readonly ItemIcons _icons;

		private readonly Customizer _customizer;

		private readonly ItemTooltipViewModelFactory _tooltipViewModelFactory;

		private bool _isSelected;

		public Item Item { get; }

		public Color Color { get; }

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				SetField(ref _isSelected, value, "IsSelected");
			}
		}

		public string SelectLabel => (string)_localizer["Select"];

		public string DeselectLabel => (string)_localizer["Deselect"];

		public RelayCommand ToggleCommand => new RelayCommand(delegate
		{
			IsSelected = !IsSelected;
		});

		public string CopyNameLabel => (string)_localizer["Copy Name"];

		public RelayCommand CopyNameCommand => new RelayCommand(delegate
		{
			_clipboard.SetText(Item.Name);
		});

		public string CopyChatLinkLabel => (string)_localizer["Copy Chat Link"];

		public RelayCommand CopyChatLinkCommand => new RelayCommand(delegate
		{
			_clipboard.SetText(Item.ChatLink);
		});

		public string OpenWikiLabel => (string)_localizer["Open Wiki"];

		public RelayCommand OpenWikiCommand => new RelayCommand(delegate
		{
			Process.Start(_localizer["Wiki search", new object[1] { WebUtility.UrlEncode(Item.ChatLink) }]);
		});

		public string OpenApiLabel => (string)_localizer["Open API"];

		public RelayCommand OpenApiCommand => new RelayCommand(delegate
		{
			Process.Start(_localizer["Item API", new object[1] { Item.Id }]);
		});

		public ItemsListViewModel(IStringLocalizer<ItemsList> localizer, IEventAggregator eventAggregator, IClipBoard clipboard, ItemIcons icons, Customizer customizer, Item item, ItemTooltipViewModelFactory tooltipViewModelFactory, bool isSelected)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			_localizer = localizer;
			_eventAggregator = eventAggregator;
			_clipboard = clipboard;
			_icons = icons;
			_customizer = customizer;
			_tooltipViewModelFactory = tooltipViewModelFactory;
			_isSelected = isSelected;
			Item = item ?? throw new ArgumentNullException("item");
			Color = ItemColors.Rarity(item.Rarity);
			_eventAggregator.Subscribe(new Action<LocaleChanged>(OnLocaleChanged));
		}

		private void OnLocaleChanged(LocaleChanged changed)
		{
			OnPropertyChanged("SelectLabel");
			OnPropertyChanged("DeselectLabel");
			OnPropertyChanged("CopyNameLabel");
			OnPropertyChanged("CopyChatLinkLabel");
			OnPropertyChanged("OpenWikiLabel");
			OnPropertyChanged("OpenApiLabel");
		}

		public AsyncTexture2D? GetIcon()
		{
			return _icons.GetIcon(Item);
		}

		public ItemTooltipViewModel CreateTooltipViewModel()
		{
			List<UpgradeSlot> upgrades = new List<UpgradeSlot>();
			IUpgradable upgradable = Item as IUpgradable;
			if (upgradable != null)
			{
				upgrades.AddRange(UpgradeSlots(upgradable));
				upgrades.AddRange(InfusionSlots(upgradable));
			}
			return _tooltipViewModelFactory.Create(Item, 1, upgrades);
		}

		private IEnumerable<UpgradeSlot> UpgradeSlots(IUpgradable upgradable)
		{
			return upgradable.UpgradeSlots.Select((int? upgradeComponentId) => new UpgradeSlot
			{
				Type = UpgradeSlotType.Default,
				UpgradeComponent = _customizer.GetUpgradeComponent(upgradeComponentId)
			});
		}

		private IEnumerable<UpgradeSlot> InfusionSlots(IUpgradable upgradable)
		{
			return upgradable.InfusionSlots.Select(delegate(InfusionSlot infusionSlot)
			{
				UpgradeSlot upgradeSlot = new UpgradeSlot();
				InfusionSlotFlags flags = infusionSlot.Flags;
				if ((object)flags == null)
				{
					goto IL_002a;
				}
				UpgradeSlotType type;
				if (!flags.Infusion)
				{
					if (!flags.Enrichment)
					{
						goto IL_002a;
					}
					type = UpgradeSlotType.Enrichment;
				}
				else
				{
					type = UpgradeSlotType.Infusion;
				}
				goto IL_002c;
				IL_002c:
				upgradeSlot.Type = type;
				upgradeSlot.UpgradeComponent = _customizer.GetUpgradeComponent(infusionSlot.ItemId);
				return upgradeSlot;
				IL_002a:
				type = UpgradeSlotType.Default;
				goto IL_002c;
			});
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<LocaleChanged>(new Action<LocaleChanged>(OnLocaleChanged));
		}
	}
}
