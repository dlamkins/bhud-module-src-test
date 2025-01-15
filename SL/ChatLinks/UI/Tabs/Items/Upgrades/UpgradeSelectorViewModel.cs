using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GuildWars2;
using GuildWars2.Items;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeSelectorViewModel : ViewModel
	{
		[CompilerGenerated]
		private Customizer _003Ccustomizer_003EP;

		[CompilerGenerated]
		private ItemsListViewModelFactory _003CitemsListViewModelFactory_003EP;

		[CompilerGenerated]
		private Item _003Ctarget_003EP;

		[CompilerGenerated]
		private UpgradeSlotType _003CslotType_003EP;

		[CompilerGenerated]
		private UpgradeComponent? _003CselectedUpgradeComponent_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		private List<IGrouping<string, ItemsListViewModel>>? _options;

		public IReadOnlyList<IGrouping<string, ItemsListViewModel>> Options => _options ?? (_options = GetOptions());

		public IEnumerable<ItemsListViewModel> AllOptions => Options.SelectMany((IGrouping<string, ItemsListViewModel> group) => group);

		public RelayCommand<ItemsListViewModel> SelectCommand => new RelayCommand<ItemsListViewModel>(new Action<ItemsListViewModel>(OnSelect));

		public RelayCommand DeselectCommand => new RelayCommand(new Action(OnRemove));

		public RelayCommand MouseEnteredUpgradeSelectorCommand => new RelayCommand(new Action(OnMouseEnteredUpgradeSelector));

		public RelayCommand MouseLeftUpgradeSelectorCommand => new RelayCommand(new Action(OnMouseLeftUpgradeSelector));

		public event EventHandler<UpgradeComponent>? Selected;

		public event EventHandler? Deselected;

		public UpgradeSelectorViewModel(Customizer customizer, ItemsListViewModelFactory itemsListViewModelFactory, Item target, UpgradeSlotType slotType, UpgradeComponent? selectedUpgradeComponent, IEventAggregator eventAggregator)
		{
			_003Ccustomizer_003EP = customizer;
			_003CitemsListViewModelFactory_003EP = itemsListViewModelFactory;
			_003Ctarget_003EP = target;
			_003CslotType_003EP = slotType;
			_003CselectedUpgradeComponent_003EP = selectedUpgradeComponent;
			_003CeventAggregator_003EP = eventAggregator;
			base._002Ector();
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
			_003CeventAggregator_003EP.Publish(new MouseEnteredUpgradeSelector());
		}

		private void OnMouseLeftUpgradeSelector()
		{
			_003CeventAggregator_003EP.Publish(new MouseLeftUpgradeSelector());
		}

		private List<IGrouping<string, ItemsListViewModel>> GetOptions()
		{
			Dictionary<string, int> groupOrder = new Dictionary<string, int>
			{
				{ "Runes", 1 },
				{ "Sigils", 1 },
				{ "Runes (PvP)", 2 },
				{ "Sigils (PvP)", 2 },
				{ "Infusions", 3 },
				{ "Enrichments", 3 },
				{ "Universal Upgrades", 4 },
				{ "Uncategorized", 5 }
			};
			return (from grouped in (from _003C_003Eh__TransparentIdentifier0 in _003Ccustomizer_003EP.GetUpgradeComponents(_003Ctarget_003EP, _003CslotType_003EP).Select(delegate(UpgradeComponent upgrade)
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
					let vm = _003CitemsListViewModelFactory_003EP.Create(upgrade, upgrade == _003CselectedUpgradeComponent_003EP)
					orderby rank, upgrade.Level, upgrade.Name
					select _003C_003Eh__TransparentIdentifier1).GroupBy(_003C_003Eh__TransparentIdentifier1 =>
				{
					UpgradeComponent upgrade2 = _003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade;
					if (upgrade2 is Gem)
					{
						return "Universal Upgrades";
					}
					if (upgrade2 is Rune)
					{
						if (_003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade.GameTypes.All(delegate(Extensible<GameType> type)
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
						}))
						{
							return "Runes (PvP)";
						}
						return "Runes";
					}
					if (upgrade2 is Sigil)
					{
						if (_003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade.GameTypes.All(delegate(Extensible<GameType> type)
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
						}))
						{
							return "Sigils (PvP)";
						}
						return "Sigils";
					}
					if (_003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade.InfusionUpgradeFlags.Infusion)
					{
						return "Infusions";
					}
					return _003C_003Eh__TransparentIdentifier1._003C_003Eh__TransparentIdentifier0.upgrade.InfusionUpgradeFlags.Enrichment ? "Enrichments" : "Uncategorized";
				}, _003C_003Eh__TransparentIdentifier1 => _003C_003Eh__TransparentIdentifier1.vm)
				orderby groupOrder[grouped.Key]
				select grouped).ToList();
		}
	}
}
