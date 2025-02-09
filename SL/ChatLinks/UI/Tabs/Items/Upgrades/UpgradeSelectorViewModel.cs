using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GuildWars2;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeSelectorViewModel : ViewModel, IDisposable
	{
		private ObservableCollection<IGrouping<string, ItemsListViewModel>>? _options;

		private readonly IStringLocalizer<UpgradeSelector> _localizer;

		private readonly Customizer _customizer;

		private readonly ItemsListViewModelFactory _itemsListViewModelFactory;

		private readonly Item _target;

		private readonly UpgradeSlotType _slotType;

		private readonly UpgradeComponent? _selectedUpgradeComponent;

		private readonly IEventAggregator _eventAggregator;

		public ObservableCollection<IGrouping<string, ItemsListViewModel>> Options
		{
			get
			{
				return _options ?? (_options = GetOptions());
			}
			private set
			{
				SetField(ref _options, value, "Options");
			}
		}

		public IEnumerable<ItemsListViewModel> AllOptions => Options.SelectMany((IGrouping<string, ItemsListViewModel> group) => group);

		public RelayCommand<ItemsListViewModel> SelectCommand => new RelayCommand<ItemsListViewModel>(new Action<ItemsListViewModel>(OnSelect));

		public RelayCommand DeselectCommand => new RelayCommand(new Action(OnRemove));

		public RelayCommand MouseEnteredUpgradeSelectorCommand => new RelayCommand(new Action(OnMouseEnteredUpgradeSelector));

		public RelayCommand MouseLeftUpgradeSelectorCommand => new RelayCommand(new Action(OnMouseLeftUpgradeSelector));

		public event EventHandler<UpgradeComponent>? Selected;

		public event EventHandler? Deselected;

		public UpgradeSelectorViewModel(IStringLocalizer<UpgradeSelector> localizer, Customizer customizer, ItemsListViewModelFactory itemsListViewModelFactory, Item target, UpgradeSlotType slotType, UpgradeComponent? selectedUpgradeComponent, IEventAggregator eventAggregator)
		{
			_localizer = localizer;
			_customizer = customizer;
			_itemsListViewModelFactory = itemsListViewModelFactory;
			_target = target;
			_slotType = slotType;
			_selectedUpgradeComponent = selectedUpgradeComponent;
			_eventAggregator = eventAggregator;
			eventAggregator.Subscribe(new Action<LocaleChanged>(OnLocaleChanged));
		}

		private void OnLocaleChanged(LocaleChanged changed)
		{
			Options = null;
		}

		private void OnSelect(ItemsListViewModel selection)
		{
			foreach (ItemsListViewModel item in AllOptions.Where((ItemsListViewModel o) => o.IsSelected))
			{
				item.IsSelected = item == selection;
			}
			this.Selected?.Invoke(this, (UpgradeComponent)selection.Item);
		}

		private void OnRemove()
		{
			this.Deselected?.Invoke(this, EventArgs.Empty);
		}

		private void OnMouseEnteredUpgradeSelector()
		{
			_eventAggregator.Publish(new MouseEnteredUpgradeSelector());
		}

		private void OnMouseLeftUpgradeSelector()
		{
			_eventAggregator.Publish(new MouseLeftUpgradeSelector());
		}

		private ObservableCollection<IGrouping<string, ItemsListViewModel>> GetOptions()
		{
			Dictionary<string, int> groupOrder = new Dictionary<string, int>
			{
				{
					_localizer["Runes"],
					1
				},
				{
					_localizer["Sigils"],
					1
				},
				{
					_localizer["Runes (PvP)"],
					2
				},
				{
					_localizer["Sigils (PvP)"],
					2
				},
				{
					_localizer["Infusions"],
					3
				},
				{
					_localizer["Enrichments"],
					3
				},
				{
					_localizer["Universal Upgrades"],
					4
				},
				{
					_localizer["Uncategorized"],
					5
				}
			};
			IOrderedEnumerable<IGrouping<string, ItemsListViewModel>> orderedEnumerable = from grouped in (from _003C_003Eh__TransparentIdentifier0 in _customizer.GetUpgradeComponents(_target, _slotType).Select(delegate(UpgradeComponent upgrade)
					{
						int rank = ((!upgrade.Rarity.IsDefined()) ? 99 : (upgrade.Rarity.ToEnum() switch
						{
							Rarity.Junk => 0, 
							Rarity.Basic => 1, 
							Rarity.Fine => 2, 
							Rarity.Masterwork => 3, 
							Rarity.Rare => 4, 
							Rarity.Exotic => 5, 
							Rarity.Ascended => 6, 
							Rarity.Legendary => 7, 
							_ => 99, 
						}));
						return new { upgrade, rank };
					})
					let vm = _itemsListViewModelFactory.Create(upgrade, upgrade.Id == _selectedUpgradeComponent?.Id)
					orderby rank, upgrade.Level, upgrade.Name
					select _003C_003Eh__TransparentIdentifier1).GroupBy(_003C_003Eh__TransparentIdentifier1 =>
				{
					UpgradeComponent upgrade2 = _003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade;
					LocalizedString localizedString = ((upgrade2 is Gem) ? _localizer["Universal Upgrades"] : ((!(upgrade2 is Rune)) ? ((!(upgrade2 is Sigil)) ? (_003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade.InfusionUpgradeFlags.Infusion ? _localizer["Infusions"] : ((!_003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade.InfusionUpgradeFlags.Enrichment) ? _localizer["Uncategorized"] : _localizer["Enrichments"])) : ((!_003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade.GameTypes.All(delegate(Extensible<GameType> type)
					{
						bool flag3 = type.IsDefined();
						bool flag4;
						if (flag3)
						{
							GameType? gameType2 = type.ToEnum();
							if (gameType2.HasValue)
							{
								GameType valueOrDefault2 = gameType2.GetValueOrDefault();
								if ((uint)(valueOrDefault2 - 4) <= 1u)
								{
									flag4 = true;
									goto IL_0030;
								}
							}
							flag4 = false;
							goto IL_0030;
						}
						goto IL_0032;
						IL_0032:
						return flag3;
						IL_0030:
						flag3 = flag4;
						goto IL_0032;
					})) ? _localizer["Sigils"] : _localizer["Sigils (PvP)"])) : ((!_003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade.GameTypes.All(delegate(Extensible<GameType> type)
					{
						bool flag = type.IsDefined();
						bool flag2;
						if (flag)
						{
							GameType? gameType = type.ToEnum();
							if (gameType.HasValue)
							{
								GameType valueOrDefault = gameType.GetValueOrDefault();
								if ((uint)(valueOrDefault - 4) <= 1u)
								{
									flag2 = true;
									goto IL_0030;
								}
							}
							flag2 = false;
							goto IL_0030;
						}
						goto IL_0032;
						IL_0032:
						return flag;
						IL_0030:
						flag = flag2;
						goto IL_0032;
					})) ? _localizer["Runes"] : _localizer["Runes (PvP)"])));
					return (string)localizedString;
				}, _003C_003Eh__TransparentIdentifier1 => _003C_003Eh__TransparentIdentifier1.vm)
				orderby groupOrder[grouped.Key]
				select grouped;
			ObservableCollection<IGrouping<string, ItemsListViewModel>> observableCollection = new ObservableCollection<IGrouping<string, ItemsListViewModel>>();
			foreach (IGrouping<string, ItemsListViewModel> item in orderedEnumerable)
			{
				observableCollection.Add(item);
			}
			return observableCollection;
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<LocaleChanged>(new Action<LocaleChanged>(OnLocaleChanged));
		}
	}
}
