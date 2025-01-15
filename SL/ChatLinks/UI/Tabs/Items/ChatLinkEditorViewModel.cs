using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Blish_HUD.Content;
using GuildWars2.Chat;
using GuildWars2.Items;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.ChatLinks.UI.Tabs.Items.Upgrades;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ChatLinkEditorViewModel : ViewModel
	{
		private readonly IOptionsMonitor<ChatLinkOptions> _options;

		private bool _allowScroll = true;

		private int _quantity = 1;

		private UpgradeComponent? _suffixItem;

		private UpgradeComponent? _secondarySuffixItem;

		private readonly List<UpgradeEditorViewModel> _upgradeEditorViewModels;

		private readonly ItemTooltipViewModelFactory _tooltipViewModelFactory;

		private readonly UpgradeEditorViewModelFactory _upgradeEditorViewModelFactory;

		private readonly ItemIcons _icons;

		private readonly Customizer _customizer;

		private readonly IClipBoard _clipboard;

		private bool _showInfusionWarning;

		private int _maxStackSize;

		public int MaxStackSize
		{
			get
			{
				return _maxStackSize;
			}
			set
			{
				SetField(ref _maxStackSize, value, "MaxStackSize");
			}
		}

		public bool AllowScroll
		{
			get
			{
				return _allowScroll;
			}
			set
			{
				SetField(ref _allowScroll, value, "AllowScroll");
			}
		}

		public bool ShowInfusionWarning
		{
			get
			{
				return _showInfusionWarning;
			}
			private set
			{
				SetField(ref _showInfusionWarning, value, "ShowInfusionWarning");
			}
		}

		public Item Item { get; }

		public ObservableCollection<UpgradeEditorViewModel> UpgradeEditorViewModels => new ObservableCollection<UpgradeEditorViewModel>(_upgradeEditorViewModels);

		public string ItemName
		{
			get
			{
				string name = Item.Name;
				if (!Item.Flags.HideSuffix)
				{
					UpgradeComponent defaultSuffix = _customizer.DefaultSuffixItem(Item);
					if (!string.IsNullOrEmpty(defaultSuffix?.SuffixName) && name.EndsWith(defaultSuffix.SuffixName))
					{
						string text = name;
						int length = defaultSuffix.SuffixName.Length;
						name = text.Substring(0, text.Length - length);
						name = name.TrimEnd();
					}
					string newSuffix = SuffixName ?? defaultSuffix?.SuffixName;
					if (!string.IsNullOrEmpty(newSuffix))
					{
						name = name + " " + newSuffix;
					}
				}
				if (Quantity > 1)
				{
					name = $"{Quantity} {name}";
				}
				return name;
			}
		}

		public string? SuffixName => UpgradeEditorViewModels.FirstOrDefault(delegate(UpgradeEditorViewModel u)
		{
			if (u != null)
			{
				UpgradeSlotViewModel upgradeSlotViewModel = u.UpgradeSlotViewModel;
				if (upgradeSlotViewModel != null && upgradeSlotViewModel.Type == UpgradeSlotType.Default)
				{
					return (object)upgradeSlotViewModel.SelectedUpgradeComponent != null;
				}
			}
			return false;
		})?.UpgradeSlotViewModel.SelectedUpgradeComponent?.SuffixName;

		public Color ItemNameColor { get; }

		public int Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				OnPropertyChanging("ItemName");
				OnPropertyChanging("ChatLink");
				SetField(ref _quantity, value, "Quantity");
				OnPropertyChanged("ItemName");
				OnPropertyChanged("ChatLink");
			}
		}

		public UpgradeComponent? SuffixItem
		{
			get
			{
				return _suffixItem;
			}
			set
			{
				OnPropertyChanging("ItemName");
				OnPropertyChanging("ChatLink");
				SetField(ref _suffixItem, value, "SuffixItem");
				OnPropertyChanged("ItemName");
				OnPropertyChanged("ChatLink");
			}
		}

		public UpgradeComponent? SecondarySuffixItem
		{
			get
			{
				return _secondarySuffixItem;
			}
			set
			{
				OnPropertyChanging("ItemName");
				OnPropertyChanging("ChatLink");
				SetField(ref _secondarySuffixItem, value, "SecondarySuffixItem");
				OnPropertyChanged("ItemName");
				OnPropertyChanged("ChatLink");
			}
		}

		public string ChatLink
		{
			get
			{
				return new ItemLink
				{
					ItemId = Item.Id,
					Count = Quantity,
					SuffixItemId = SuffixItem?.Id,
					SecondarySuffixItemId = SecondarySuffixItem?.Id
				}.ToString();
			}
			set
			{
			}
		}

		public RelayCommand CopyNameCommand => new RelayCommand(delegate
		{
			_clipboard.SetText(Item.Name);
		});

		public RelayCommand CopyChatLinkCommand => new RelayCommand(delegate
		{
			_clipboard.SetText(ChatLink);
		});

		public RelayCommand OpenWikiCommand => new RelayCommand(delegate
		{
			Process.Start("https://wiki.guildwars2.com/wiki/?search=" + WebUtility.UrlEncode(Item.ChatLink));
		});

		public RelayCommand OpenApiCommand => new RelayCommand(delegate
		{
			Process.Start($"https://api.guildwars2.com/v2/items/{Item.Id}?v=latest");
		});

		public RelayCommand MinQuantityCommand => new RelayCommand(delegate
		{
			Quantity = 1;
		});

		public RelayCommand MaxQuantityCommand => new RelayCommand(delegate
		{
			Quantity = 250;
		});

		public ChatLinkEditorViewModel(IOptionsMonitor<ChatLinkOptions> options, IEventAggregator eventAggregator, ItemTooltipViewModelFactory tooltipViewModelFactory, UpgradeEditorViewModelFactory upgradeEditorViewModelFactory, ItemIcons icons, Customizer customizer, IClipBoard clipboard, Item item)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			_options = options;
			_tooltipViewModelFactory = tooltipViewModelFactory;
			_upgradeEditorViewModelFactory = upgradeEditorViewModelFactory;
			_icons = icons;
			_customizer = customizer;
			_clipboard = clipboard;
			Item = item;
			ItemNameColor = ItemColors.Rarity(item.Rarity);
			_upgradeEditorViewModels = CreateUpgradeEditorViewModels().ToList();
			foreach (var item2 in _upgradeEditorViewModels.Select((UpgradeEditorViewModel vm, int index) => (index + 1, vm)))
			{
				var (slot, vm2) = item2;
				vm2.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == "Customizing" && vm2.Customizing)
					{
						foreach (UpgradeEditorViewModel upgradeEditorViewModel in UpgradeEditorViewModels)
						{
							upgradeEditorViewModel.Customizing = upgradeEditorViewModel == sender;
						}
					}
				};
				vm2.UpgradeSlotViewModel.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == "SelectedUpgradeComponent" && vm2.UpgradeSlotViewModel.Type == UpgradeSlotType.Default)
					{
						switch (slot)
						{
						case 1:
							SuffixItem = vm2.UpgradeSlotViewModel.SelectedUpgradeComponent;
							break;
						case 2:
							SecondarySuffixItem = vm2.UpgradeSlotViewModel.SelectedUpgradeComponent;
							break;
						}
					}
				};
			}
			MaxStackSize = (options.CurrentValue.RaiseStackSize ? 255 : 250);
			options.OnChange(delegate(ChatLinkOptions current)
			{
				MaxStackSize = (current.RaiseStackSize ? 255 : 250);
			});
			eventAggregator.Subscribe(new Action<MouseEnteredUpgradeSelector>(OnMouseEnteredUpgradeSelector));
			eventAggregator.Subscribe(new Action<MouseLeftUpgradeSelector>(OnMouseLeftUpgradeSelector));
			eventAggregator.Subscribe(new Action<UpgradeSlotChanged>(OnUpgradeSlotChanged));
		}

		private void OnUpgradeSlotChanged(UpgradeSlotChanged obj)
		{
			ShowInfusionWarning = UpgradeEditorViewModels.Where((UpgradeEditorViewModel editor) => editor.UpgradeSlotType != UpgradeSlotType.Default).Any((UpgradeEditorViewModel editor) => (object)editor.UpgradeSlotViewModel.SelectedUpgradeComponent != null);
		}

		private void OnMouseEnteredUpgradeSelector(MouseEnteredUpgradeSelector obj)
		{
			AllowScroll = false;
		}

		private void OnMouseLeftUpgradeSelector(MouseLeftUpgradeSelector obj)
		{
			AllowScroll = true;
		}

		public ItemTooltipViewModel CreateTooltipViewModel()
		{
			IEnumerable<SL.ChatLinks.UI.Tabs.Items.Tooltips.UpgradeSlot> upgrades = UpgradeEditorViewModels.Select((UpgradeEditorViewModel vm) => new SL.ChatLinks.UI.Tabs.Items.Tooltips.UpgradeSlot
			{
				Type = vm.UpgradeSlotType,
				UpgradeComponent = vm.EffectiveUpgradeComponent
			});
			return _tooltipViewModelFactory.Create(Item, Quantity, upgrades);
		}

		public AsyncTexture2D? GetIcon()
		{
			return _icons.GetIcon(Item);
		}

		private IEnumerable<UpgradeEditorViewModel> CreateUpgradeEditorViewModels()
		{
			Item item = Item;
			IUpgradable upgradable = item as IUpgradable;
			if (upgradable == null)
			{
				if (_options.CurrentValue.BananaMode)
				{
					yield return _upgradeEditorViewModelFactory.Create(Item, UpgradeSlotType.Default, null);
				}
				yield break;
			}
			foreach (int? defaultUpgradeComponentId in upgradable.UpgradeSlots)
			{
				yield return _upgradeEditorViewModelFactory.Create(Item, UpgradeSlotType.Default, defaultUpgradeComponentId);
			}
			foreach (InfusionSlot infusionSlot in upgradable.InfusionSlots)
			{
				UpgradeEditorViewModelFactory upgradeEditorViewModelFactory = _upgradeEditorViewModelFactory;
				item = Item;
				InfusionSlotFlags flags = infusionSlot.Flags;
				if ((object)flags == null)
				{
					goto IL_017b;
				}
				UpgradeSlotType slotType;
				if (!flags.Enrichment)
				{
					if (!flags.Infusion)
					{
						goto IL_017b;
					}
					slotType = UpgradeSlotType.Infusion;
				}
				else
				{
					slotType = UpgradeSlotType.Enrichment;
				}
				goto IL_017e;
				IL_017b:
				slotType = UpgradeSlotType.Default;
				goto IL_017e;
				IL_017e:
				yield return upgradeEditorViewModelFactory.Create(item, slotType, infusionSlot.ItemId);
			}
		}
	}
}
