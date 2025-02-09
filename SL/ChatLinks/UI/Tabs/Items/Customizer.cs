using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using SL.ChatLinks.Storage;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class Customizer
	{
		[CompilerGenerated]
		private IDbContextFactory _003CcontextFactory_003EP;

		[CompilerGenerated]
		private ILocale _003Clocale_003EP;

		public Customizer(IDbContextFactory contextFactory, ILocale locale)
		{
			_003CcontextFactory_003EP = contextFactory;
			_003Clocale_003EP = locale;
			base._002Ector();
		}

		public UpgradeComponent? DefaultSuffixItem(Item item)
		{
			IUpgradable upgradable = item as IUpgradable;
			if (upgradable == null)
			{
				return null;
			}
			return GetUpgradeComponent(upgradable.SuffixItemId);
		}

		public async ValueTask<UpgradeComponent?> GetUpgradeComponentAsync(int? upgradeComponentId)
		{
			if (!upgradeComponentId.HasValue)
			{
				return null;
			}
			await using ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			return await context.Items.OfType<UpgradeComponent>().SingleOrDefaultAsync((UpgradeComponent item) => item.Id == upgradeComponentId.Value);
		}

		public UpgradeComponent? GetUpgradeComponent(int? upgradeComponentId)
		{
			if (!upgradeComponentId.HasValue)
			{
				return null;
			}
			using ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			return context.Items.OfType<UpgradeComponent>().SingleOrDefault((UpgradeComponent item) => item.Id == upgradeComponentId.Value);
		}

		public IEnumerable<UpgradeComponent> GetUpgradeComponents(Item targetItem, UpgradeSlotType slotType)
		{
			using ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			IQueryable<UpgradeComponent> source;
			switch (slotType)
			{
			case UpgradeSlotType.Infusion:
				source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\n  AND InfusionUpgradeFlags -> '$.infusion' = 'true'");
				break;
			case UpgradeSlotType.Enrichment:
				source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'upgrade_component'\r\n  AND InfusionUpgradeFlags -> '$.enrichment' = 'true'");
				break;
			case UpgradeSlotType.Default:
			{
				if (targetItem is Trinket)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem')\r\n\tAND UpgradeComponentFlags -> '$.Trinket' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Backpack)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type = 'gem'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				Armor armor3 = targetItem as Armor;
				if ((object)armor3 != null && armor3.WeightClass == WeightClass.Heavy)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'rune')\r\n\tAND UpgradeComponentFlags -> '$.HeavyArmor' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				Armor armor2 = targetItem as Armor;
				if ((object)armor2 != null && armor2.WeightClass == WeightClass.Medium)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'rune')\r\n\tAND UpgradeComponentFlags -> '$.MediumArmor' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				Armor armor = targetItem as Armor;
				if ((object)armor != null && armor.WeightClass == WeightClass.Light)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'rune')\r\n\tAND UpgradeComponentFlags -> '$.LightArmor' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Axe)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Axe' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Dagger)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Dagger' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Focus)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Focus' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Greatsword)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Greatsword' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Hammer)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Hammer' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is HarpoonGun)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.HarpoonGun' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Longbow)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.LongBow' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Mace)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Mace' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Pistol)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Pistol' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Rifle)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Rifle' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Scepter)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Scepter' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Shield)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Shield' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Shortbow)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.ShortBow' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Spear)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Spear' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Staff)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Staff' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Sword)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Sword' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Torch)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Torch' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Trident)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Trident' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				if (targetItem is Warhorn)
				{
					source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'sigil')\r\n\tAND UpgradeComponentFlags -> '$.Warhorn' = 'true'\r\n\tAND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n\tAND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
					break;
				}
				goto default;
			}
			default:
				source = context.Set<UpgradeComponent>().FromSqlRaw("SELECT *\r\nFROM Items\r\nWHERE Type in ('upgrade_component', 'gem', 'rune', 'sigil')\r\n  AND InfusionUpgradeFlags -> '$.infusion' = 'false'\r\n  AND InfusionUpgradeFlags -> '$.enrichment' = 'false'");
				break;
			}
			return source.ToImmutableList();
		}
	}
}
