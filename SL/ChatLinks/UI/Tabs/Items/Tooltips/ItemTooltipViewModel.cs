using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD.Content;
using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items.Tooltips
{
	public sealed class ItemTooltipViewModel : ViewModel
	{
		[CompilerGenerated]
		private ItemIcons _003Cicons_003EP;

		public IReadOnlyList<UpgradeSlot> UpgradesSlots { get; }

		public Item Item { get; }

		public int Quantity { get; }

		public string? DefaultSuffixName { get; }

		public Color ItemNameColor { get; }

		public string ItemName
		{
			get
			{
				string name = Item.Name;
				if (!Item.Flags.HideSuffix)
				{
					if (!string.IsNullOrEmpty(DefaultSuffixName) && name.EndsWith(DefaultSuffixName))
					{
						string text = name;
						int length = DefaultSuffixName!.Length;
						name = text.Substring(0, text.Length - length);
						name = name.TrimEnd();
					}
					string newSuffix = SuffixName ?? DefaultSuffixName;
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

		public string? SuffixName => UpgradesSlots.FirstOrDefault((UpgradeSlot u) => u != null && u.Type == UpgradeSlotType.Default && (object)u.UpgradeComponent != null)?.UpgradeComponent?.SuffixName ?? DefaultSuffixName;

		public Coin TotalVendorValue => Item.VendorValue * Quantity;

		public ItemTooltipViewModel(Item item, int quantity, IEnumerable<UpgradeSlot> upgrades, ItemIcons icons, Customizer customizer)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			_003Cicons_003EP = icons;
			UpgradesSlots = upgrades.ToList();
			Item = item;
			Quantity = quantity;
			DefaultSuffixName = customizer.DefaultSuffixItem(item)?.SuffixName;
			ItemNameColor = ItemColors.Rarity(item.Rarity);
			base._002Ector();
		}

		public string AttributeName(Extensible<AttributeName> stat)
		{
			if (stat.IsDefined())
			{
				return stat.ToEnum() switch
				{
					GuildWars2.Hero.AttributeName.Power => "Power", 
					GuildWars2.Hero.AttributeName.Precision => "Precision", 
					GuildWars2.Hero.AttributeName.Toughness => "Toughness", 
					GuildWars2.Hero.AttributeName.Vitality => "Vitality", 
					GuildWars2.Hero.AttributeName.Concentration => "Concentration", 
					GuildWars2.Hero.AttributeName.ConditionDamage => "Condition Damage", 
					GuildWars2.Hero.AttributeName.Expertise => "Expertise", 
					GuildWars2.Hero.AttributeName.Ferocity => "Ferocity", 
					GuildWars2.Hero.AttributeName.HealingPower => "Healing Power", 
					GuildWars2.Hero.AttributeName.AgonyResistance => "Agony Resistance", 
					_ => stat.ToString(), 
				};
			}
			return stat.ToString();
		}

		public AsyncTexture2D? GetIcon(Item item)
		{
			return _003Cicons_003EP.GetIcon(item);
		}
	}
}
