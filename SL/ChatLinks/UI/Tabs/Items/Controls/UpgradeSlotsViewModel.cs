using System;
using System.Collections.Generic;
using System.Linq;
using GuildWars2.Hero;
using GuildWars2.Items;
using SL.Common;
using SL.Common.Controls.Items.Upgrades;

namespace SL.ChatLinks.UI.Tabs.Items.Controls
{
	public sealed class UpgradeSlotsViewModel : ViewModel
	{
		public Item? Item { get; set; }

		public IReadOnlyDictionary<int, UpgradeComponent>? UpgradeComponents { get; set; }

		public IReadOnlyList<UpgradeSlotModel>? UpgradeSlots { get; private set; }

		public IReadOnlyList<UpgradeComponent>? UpgradeOptions { get; private set; }

		public IReadOnlyList<UpgradeSlotModel>? InfusionSlots { get; private set; }

		public IReadOnlyList<UpgradeComponent>? InfusionOptions { get; private set; }

		public IReadOnlyList<UpgradeComponent>? EnrichmentOptions { get; private set; }

		public int? SuffixItemId => UpgradeSlots?.FirstOrDefault()?.SelectedUpgradeComponent?.Id;

		public int? SecondarySuffixItemId => UpgradeSlots?.Skip(1).FirstOrDefault()?.SelectedUpgradeComponent?.Id;

		public UpgradeSlotModel? UpgradeSlot1 => UpgradeSlots?.FirstOrDefault();

		public UpgradeSlotModel? UpgradeSlot2 => UpgradeSlots?.Skip(1).FirstOrDefault();

		public event Action? UpgradesChanged;

		public void Initialize()
		{
			if ((object)Item == null || UpgradeComponents == null)
			{
				throw new InvalidOperationException();
			}
			UpgradeSlots = SetupUpgradeSlots(Item);
			InfusionSlots = SetupInfusionSlots(Item);
			UpgradeOptions = UpgradeComponents!.Values.Where((UpgradeComponent component) => FilterUpgradeSlot(UpgradeSlotType.Default, component)).ToList().AsReadOnly();
			InfusionOptions = UpgradeComponents!.Values.Where((UpgradeComponent component) => FilterUpgradeSlot(UpgradeSlotType.Infusion, component)).ToList().AsReadOnly();
			EnrichmentOptions = UpgradeComponents!.Values.Where((UpgradeComponent component) => FilterUpgradeSlot(UpgradeSlotType.Enrichment, component)).ToList().AsReadOnly();
		}

		public IEnumerable<UpgradeSlotModel> Slots()
		{
			if (UpgradeSlots == null || InfusionSlots == null)
			{
				throw new InvalidOperationException();
			}
			return UpgradeSlots.Concat<UpgradeSlotModel>(InfusionSlots);
		}

		public UpgradeComponent? EffectiveSuffixItem()
		{
			if (UpgradeSlots == null)
			{
				throw new InvalidOperationException();
			}
			return UpgradeSlots.Select((UpgradeSlotModel pair) => pair.EffectiveUpgrade).FirstOrDefault((UpgradeComponent upgrade) => (object)upgrade != null);
		}

		public IReadOnlyList<InfusionSlot> Infusions()
		{
			if (InfusionSlots == null)
			{
				throw new InvalidOperationException();
			}
			return InfusionSlots.Select((UpgradeSlotModel slot) => new InfusionSlot
			{
				ItemId = slot.EffectiveUpgrade?.Id,
				Flags = new InfusionSlotFlags
				{
					Infusion = (slot.Type == UpgradeSlotType.Infusion),
					Enrichment = (slot.Type == UpgradeSlotType.Enrichment),
					Other = Array.Empty<string>()
				}
			}).ToList().AsReadOnly();
		}

		private IReadOnlyList<UpgradeSlotModel> SetupUpgradeSlots(Item item)
		{
			if (UpgradeComponents == null)
			{
				throw new InvalidOperationException();
			}
			IUpgradable upgradable = item as IUpgradable;
			if (upgradable == null)
			{
				return Array.Empty<UpgradeSlotModel>();
			}
			return upgradable.UpgradeSlots.Select((int? upgradeComponentId) => new UpgradeSlotModel
			{
				Type = UpgradeSlotType.Default,
				DefaultUpgradeComponent = DefaultUpgradeComponent(upgradeComponentId)
			}).ToList().AsReadOnly();
		}

		private IReadOnlyList<UpgradeSlotModel> SetupInfusionSlots(Item item)
		{
			if (UpgradeComponents == null)
			{
				throw new InvalidOperationException();
			}
			IUpgradable upgradable = item as IUpgradable;
			if (upgradable == null)
			{
				return Array.Empty<UpgradeSlotModel>();
			}
			return upgradable.InfusionSlots.Select(delegate(InfusionSlot infusionSlot)
			{
				UpgradeSlotModel upgradeSlotModel = new UpgradeSlotModel();
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
				upgradeSlotModel.Type = type;
				upgradeSlotModel.DefaultUpgradeComponent = DefaultUpgradeComponent(infusionSlot.ItemId);
				return upgradeSlotModel;
				IL_002a:
				type = UpgradeSlotType.Default;
				goto IL_002c;
			}).ToList().AsReadOnly();
		}

		public UpgradeComponent? DefaultUpgradeComponent(int? upgradeComponentId)
		{
			if (UpgradeComponents == null)
			{
				throw new InvalidOperationException();
			}
			if (upgradeComponentId.HasValue && UpgradeComponents!.TryGetValue(upgradeComponentId.Value, out var upgradeComponent))
			{
				return upgradeComponent;
			}
			return null;
		}

		private bool FilterUpgradeSlot(UpgradeSlotType slotType, UpgradeComponent component)
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
			Item item = Item;
			Armor armor = item as Armor;
			if ((object)armor == null)
			{
				if (item is Axe)
				{
					return component.UpgradeComponentFlags.Axe;
				}
				if (item is Dagger)
				{
					return component.UpgradeComponentFlags.Dagger;
				}
				if (item is Focus)
				{
					return component.UpgradeComponentFlags.Focus;
				}
				if (item is Greatsword)
				{
					return component.UpgradeComponentFlags.Greatsword;
				}
				if (item is Hammer)
				{
					return component.UpgradeComponentFlags.Hammer;
				}
				if (item is HarpoonGun)
				{
					return component.UpgradeComponentFlags.HarpoonGun;
				}
				if (item is Longbow)
				{
					return component.UpgradeComponentFlags.LongBow;
				}
				if (item is Mace)
				{
					return component.UpgradeComponentFlags.Mace;
				}
				if (item is Pistol)
				{
					return component.UpgradeComponentFlags.Pistol;
				}
				if (item is Rifle)
				{
					return component.UpgradeComponentFlags.Rifle;
				}
				if (item is Scepter)
				{
					return component.UpgradeComponentFlags.Scepter;
				}
				if (item is Shield)
				{
					return component.UpgradeComponentFlags.Shield;
				}
				if (item is Shortbow)
				{
					return component.UpgradeComponentFlags.ShortBow;
				}
				if (item is Spear)
				{
					return component.UpgradeComponentFlags.Spear;
				}
				if (item is Staff)
				{
					return component.UpgradeComponentFlags.Staff;
				}
				if (item is Sword)
				{
					return component.UpgradeComponentFlags.Sword;
				}
				if (item is Torch)
				{
					return component.UpgradeComponentFlags.Torch;
				}
				if (item is Trident)
				{
					return component.UpgradeComponentFlags.Trident;
				}
				if (item is Trinket)
				{
					return component.UpgradeComponentFlags.Trinket;
				}
				if (item is Warhorn)
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

		public void OnUpgradeChanged()
		{
			this.UpgradesChanged?.Invoke();
		}
	}
}
