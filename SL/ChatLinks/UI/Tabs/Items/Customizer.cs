using System;
using System.Collections.Generic;
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
	public sealed class Customizer : IDisposable, IAsyncDisposable
	{
		[CompilerGenerated]
		private ChatLinksContext _003Ccontext_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		public IReadOnlyDictionary<int, UpgradeComponent> UpgradeComponents { get; private set; }

		public Customizer(ChatLinksContext context, IEventAggregator eventAggregator)
		{
			_003Ccontext_003EP = context;
			_003CeventAggregator_003EP = eventAggregator;
			UpgradeComponents = new Dictionary<int, UpgradeComponent>(0);
			base._002Ector();
		}

		public async Task LoadAsync()
		{
			UpgradeComponents = await _003Ccontext_003EP.Set<UpgradeComponent>().AsNoTracking().ToDictionaryAsync((UpgradeComponent upgrade) => upgrade.Id);
			_003CeventAggregator_003EP.Subscribe(new Func<DatabaseSyncCompleted, ValueTask>(OnDatabaseSyncCompleted));
		}

		private async ValueTask OnDatabaseSyncCompleted(DatabaseSyncCompleted _)
		{
			UpgradeComponents = await _003Ccontext_003EP.Set<UpgradeComponent>().AsNoTracking().ToDictionaryAsync((UpgradeComponent upgrade) => upgrade.Id);
		}

		public void Dispose()
		{
			_003CeventAggregator_003EP.Unsubscribe<DatabaseSyncCompleted>(new Func<DatabaseSyncCompleted, ValueTask>(OnDatabaseSyncCompleted));
			_003Ccontext_003EP.Dispose();
		}

		public async ValueTask DisposeAsync()
		{
			_003CeventAggregator_003EP.Unsubscribe<DatabaseSyncCompleted>(new Func<DatabaseSyncCompleted, ValueTask>(OnDatabaseSyncCompleted));
			await _003Ccontext_003EP.DisposeAsync();
		}

		public IEnumerable<UpgradeComponent> GetUpgradeComponents(Item targetItem, UpgradeSlotType slotType)
		{
			Item targetItem2 = targetItem;
			return UpgradeComponents.Values.Where((UpgradeComponent component) => FilterUpgradeSlot(targetItem2, slotType, component));
		}

		public UpgradeComponent? DefaultSuffixItem(Item item)
		{
			IUpgradable upgradable = item as IUpgradable;
			if (upgradable == null)
			{
				return null;
			}
			return GetUpgradeComponent(upgradable.SuffixItemId) ?? GetUpgradeComponent(upgradable.SecondarySuffixItemId);
		}

		public UpgradeComponent? GetUpgradeComponent(int? upgradeComponentId)
		{
			if (upgradeComponentId.HasValue && UpgradeComponents.TryGetValue(upgradeComponentId.Value, out var upgradeComponent))
			{
				return upgradeComponent;
			}
			return null;
		}

		private bool FilterUpgradeSlot(Item targetItem, UpgradeSlotType slotType, UpgradeComponent component)
		{
			if (slotType == UpgradeSlotType.Infusion)
			{
				return component.InfusionUpgradeFlags.Infusion;
			}
			if (component.InfusionUpgradeFlags.Infusion)
			{
				return false;
			}
			if (slotType == UpgradeSlotType.Enrichment)
			{
				return component.InfusionUpgradeFlags.Enrichment;
			}
			if (component.InfusionUpgradeFlags.Enrichment)
			{
				return false;
			}
			if (component is Gem)
			{
				return true;
			}
			Armor armor = targetItem as Armor;
			if ((object)armor == null)
			{
				if (targetItem is Axe)
				{
					return component.UpgradeComponentFlags.Axe;
				}
				if (targetItem is Dagger)
				{
					return component.UpgradeComponentFlags.Dagger;
				}
				if (targetItem is Focus)
				{
					return component.UpgradeComponentFlags.Focus;
				}
				if (targetItem is Greatsword)
				{
					return component.UpgradeComponentFlags.Greatsword;
				}
				if (targetItem is Hammer)
				{
					return component.UpgradeComponentFlags.Hammer;
				}
				if (targetItem is HarpoonGun)
				{
					return component.UpgradeComponentFlags.HarpoonGun;
				}
				if (targetItem is Longbow)
				{
					return component.UpgradeComponentFlags.LongBow;
				}
				if (targetItem is Mace)
				{
					return component.UpgradeComponentFlags.Mace;
				}
				if (targetItem is Pistol)
				{
					return component.UpgradeComponentFlags.Pistol;
				}
				if (targetItem is Rifle)
				{
					return component.UpgradeComponentFlags.Rifle;
				}
				if (targetItem is Scepter)
				{
					return component.UpgradeComponentFlags.Scepter;
				}
				if (targetItem is Shield)
				{
					return component.UpgradeComponentFlags.Shield;
				}
				if (targetItem is Shortbow)
				{
					return component.UpgradeComponentFlags.ShortBow;
				}
				if (targetItem is Spear)
				{
					return component.UpgradeComponentFlags.Spear;
				}
				if (targetItem is Staff)
				{
					return component.UpgradeComponentFlags.Staff;
				}
				if (targetItem is Sword)
				{
					return component.UpgradeComponentFlags.Sword;
				}
				if (targetItem is Torch)
				{
					return component.UpgradeComponentFlags.Torch;
				}
				if (targetItem is Trident)
				{
					return component.UpgradeComponentFlags.Trident;
				}
				if (targetItem is Trinket)
				{
					return component.UpgradeComponentFlags.Trinket;
				}
				if (targetItem is Warhorn)
				{
					return component.UpgradeComponentFlags.Warhorn;
				}
			}
			else
			{
				if (armor.WeightClass == WeightClass.Light)
				{
					return component.UpgradeComponentFlags.LightArmor;
				}
				if (armor.WeightClass == WeightClass.Medium)
				{
					return component.UpgradeComponentFlags.MediumArmor;
				}
				if (armor.WeightClass == WeightClass.Heavy)
				{
					return component.UpgradeComponentFlags.HeavyArmor;
				}
			}
			return true;
		}
	}
}
