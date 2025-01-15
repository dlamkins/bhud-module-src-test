using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Blish_HUD.Content;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Collections
{
	public sealed class ItemsListViewModel : ViewModel
	{
		[CompilerGenerated]
		private IClipBoard _003Cclipboard_003EP;

		[CompilerGenerated]
		private ItemIcons _003Cicons_003EP;

		[CompilerGenerated]
		private Customizer _003Ccustomizer_003EP;

		[CompilerGenerated]
		private Item _003Citem_003EP;

		[CompilerGenerated]
		private ItemTooltipViewModelFactory _003CtooltipViewModelFactory_003EP;

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

		public RelayCommand ToggleCommand => new RelayCommand(delegate
		{
			IsSelected = !IsSelected;
		});

		public RelayCommand CopyNameCommand => new RelayCommand(delegate
		{
			_003Cclipboard_003EP.SetText(Item.Name);
		});

		public RelayCommand CopyChatLinkCommand => new RelayCommand(delegate
		{
			_003Cclipboard_003EP.SetText(Item.ChatLink);
		});

		public RelayCommand OpenWikiCommand => new RelayCommand(delegate
		{
			Process.Start("https://wiki.guildwars2.com/wiki/?search=" + WebUtility.UrlEncode(_003Citem_003EP.ChatLink));
		});

		public RelayCommand OpenApiCommand => new RelayCommand(delegate
		{
			Process.Start($"https://api.guildwars2.com/v2/items/{_003Citem_003EP.Id}?v=latest");
		});

		public ItemsListViewModel(IClipBoard clipboard, ItemIcons icons, Customizer customizer, Item item, ItemTooltipViewModelFactory tooltipViewModelFactory, bool isSelected)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			_003Cclipboard_003EP = clipboard;
			_003Cicons_003EP = icons;
			_003Ccustomizer_003EP = customizer;
			_003Citem_003EP = item;
			_003CtooltipViewModelFactory_003EP = tooltipViewModelFactory;
			_isSelected = isSelected;
			Item = _003Citem_003EP ?? throw new ArgumentNullException("item");
			Color = ItemColors.Rarity(_003Citem_003EP.Rarity);
			base._002Ector();
		}

		public AsyncTexture2D? GetIcon()
		{
			return _003Cicons_003EP.GetIcon(_003Citem_003EP);
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
			return _003CtooltipViewModelFactory_003EP.Create(_003Citem_003EP, 1, upgrades);
		}

		private IEnumerable<UpgradeSlot> UpgradeSlots(IUpgradable upgradable)
		{
			return upgradable.UpgradeSlots.Select((int? upgradeComponentId) => new UpgradeSlot
			{
				Type = UpgradeSlotType.Default,
				UpgradeComponent = _003Ccustomizer_003EP.GetUpgradeComponent(upgradeComponentId)
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
				upgradeSlot.UpgradeComponent = _003Ccustomizer_003EP.GetUpgradeComponent(infusionSlot.ItemId);
				return upgradeSlot;
				IL_002a:
				type = UpgradeSlotType.Default;
				goto IL_002c;
			});
		}
	}
}
