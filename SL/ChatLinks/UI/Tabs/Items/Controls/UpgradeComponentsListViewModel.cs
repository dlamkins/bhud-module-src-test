using System;
using System.Collections.Generic;
using System.Linq;
using GuildWars2;
using GuildWars2.Items;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items.Controls
{
	public sealed class UpgradeComponentsListViewModel : ViewModel
	{
		public IReadOnlyList<UpgradeComponent>? Options { get; set; }

		public event Action<UpgradeComponent>? Selected;

		public void OnSelected(UpgradeComponent upgradeComponent)
		{
			this.Selected?.Invoke(upgradeComponent);
		}

		public IEnumerable<IGrouping<string, UpgradeComponent>> GetOptions()
		{
			if (Options == null)
			{
				throw new InvalidOperationException();
			}
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
			return from grouped in (from _003C_003Eh__TransparentIdentifier0 in Options.Select(delegate(UpgradeComponent upgrade)
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
					orderby _003C_003Eh__TransparentIdentifier0.rank, _003C_003Eh__TransparentIdentifier0.upgrade.Level, _003C_003Eh__TransparentIdentifier0.upgrade.Name
					select _003C_003Eh__TransparentIdentifier0).GroupBy(_003C_003Eh__TransparentIdentifier0 =>
				{
					UpgradeComponent upgrade2 = _003C_003Eh__TransparentIdentifier0.upgrade;
					if (upgrade2 is Gem)
					{
						return "Universal Upgrades";
					}
					if (upgrade2 is Rune)
					{
						if (_003C_003Eh__TransparentIdentifier0.upgrade.GameTypes.All(delegate(Extensible<GameType> type)
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
						if (_003C_003Eh__TransparentIdentifier0.upgrade.GameTypes.All(delegate(Extensible<GameType> type)
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
					if (_003C_003Eh__TransparentIdentifier0.upgrade.InfusionUpgradeFlags.Infusion)
					{
						return "Infusions";
					}
					return _003C_003Eh__TransparentIdentifier0.upgrade.InfusionUpgradeFlags.Enrichment ? "Enrichments" : "Uncategorized";
				}, _003C_003Eh__TransparentIdentifier0 => _003C_003Eh__TransparentIdentifier0.upgrade)
				orderby groupOrder[grouped.Key]
				select grouped;
		}
	}
}
