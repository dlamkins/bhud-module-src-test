using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage
{
	public class ChatLinksContext : DbContext
	{
		public DbSet<Item> Items => Set<Item>();

		public ChatLinksContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite("Data Source=data.db");
			}
		}

		private static string Serialize<T>(T value)
		{
			return JsonSerializer.Serialize(value);
		}

		private static T? Deserialize<T>(string value)
		{
			return JsonSerializer.Deserialize<T>(value);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			EntityTypeBuilder<Item> itemBuilder = modelBuilder.Entity<Item>();
			itemBuilder.HasDiscriminator<string>("Type").HasValue<Item>("item").HasValue<CraftingMaterial>("crafting_material")
				.HasValue<JadeTechModule>("jade_tech_module")
				.HasValue<PowerCore>("power_core")
				.HasValue<Bag>("bag")
				.HasValue<Relic>("relic")
				.HasValue<Miniature>("miniature")
				.HasValue<Trophy>("trophy")
				.HasValue<Backpack>("back")
				.HasValue<SalvageTool>("salvage_tool")
				.HasValue<Container>("container")
				.HasValue<ImmediateContainer>("immediate_container")
				.HasValue<BlackLionChest>("black_lion_chest")
				.HasValue<GiftBox>("gift_box")
				.HasValue<UpgradeComponent>("upgrade_component")
				.HasValue<Sigil>("sigil")
				.HasValue<Gem>("gem")
				.HasValue<Rune>("rune")
				.HasValue<Trinket>("trinket")
				.HasValue<Ring>("ring")
				.HasValue<Amulet>("amulet")
				.HasValue<Accessory>("accessory")
				.HasValue<Armor>("armor")
				.HasValue<Helm>("helm")
				.HasValue<HelmAquatic>("helm_aquatic")
				.HasValue<Shoulders>("shoulders")
				.HasValue<Coat>("coat")
				.HasValue<Gloves>("gloves")
				.HasValue<Leggings>("leggings")
				.HasValue<Boots>("boots")
				.HasValue<Weapon>("weapon")
				.HasValue<Greatsword>("greatsword")
				.HasValue<Torch>("torch")
				.HasValue<Staff>("staff")
				.HasValue<HarpoonGun>("harpoon_gun")
				.HasValue<Longbow>("longbow")
				.HasValue<Shortbow>("shortbow")
				.HasValue<SmallBundle>("small_bundle")
				.HasValue<LargeBundle>("large_bundle")
				.HasValue<Pistol>("pistol")
				.HasValue<Mace>("mace")
				.HasValue<Rifle>("rifle")
				.HasValue<Spear>("spear")
				.HasValue<Dagger>("dagger")
				.HasValue<Scepter>("scepter")
				.HasValue<Sword>("sword")
				.HasValue<Trident>("trident")
				.HasValue<Hammer>("hammer")
				.HasValue<Warhorn>("warhorn")
				.HasValue<Focus>("focus")
				.HasValue<Toy>("toy")
				.HasValue<ToyTwoHanded>("toy_two_handed")
				.HasValue<Axe>("axe")
				.HasValue<Shield>("shield")
				.HasValue<GatheringTool>("gathering_tool")
				.HasValue<Lure>("lure")
				.HasValue<Bait>("bait")
				.HasValue<MiningPick>("mining_pick")
				.HasValue<HarvestingSickle>("harvesting_sickle")
				.HasValue<LoggingAxe>("logging_axe")
				.HasValue<Gizmo>("gizmo")
				.HasValue<BlackLionChestKey>("black_lion_chest_key")
				.HasValue<UnlimitedConsumable>("unlimited_consumable")
				.HasValue<RentableContractNpc>("rentable_contract_npc")
				.HasValue<Consumable>("consumable")
				.HasValue<Food>("food")
				.HasValue<Utility>("utility")
				.HasValue<Transmutation>("transmutation")
				.HasValue<GenericConsumable>("generic_consumable")
				.HasValue<Currency>("currency")
				.HasValue<Booze>("booze")
				.HasValue<ContractNpc>("contract_npc")
				.HasValue<MountLicense>("mount_license")
				.HasValue<UpgradeExtractor>("upgrade_extractor")
				.HasValue<Service>("service")
				.HasValue<RandomUnlocker>("random_unlocker")
				.HasValue<HalloweenConsumable>("halloween_consumable")
				.HasValue<TeleportToFriend>("teleport_to_friend")
				.HasValue<AppearanceChanger>("appearance_changer")
				.HasValue<Unlocker>("unlocker")
				.HasValue<RecipeSheet>("recipe_sheet")
				.HasValue<BankTabExpansion>("bank_tab_expansion")
				.HasValue<BagSlotExpansion>("bag_slot_expansion")
				.HasValue<EquipmentTemplateExpansion>("equipment_template_expansion")
				.HasValue<BuildTemplateExpansion>("build_template_expansion")
				.HasValue<BuildStorageExpansion>("build_storage_expansion")
				.HasValue<Dye>("dye")
				.HasValue<StorageExpander>("storage_expander")
				.HasValue<ContentUnlocker>("content_unlocker")
				.HasValue<OutfitUnlocker>("outfit_unlocker")
				.HasValue<GliderSkinUnlocker>("glider_skin_unlocker")
				.HasValue<MountSkinUnlocker>("mount_skin_unlocker")
				.HasValue<MiniatureUnlocker>("miniature_unlocker")
				.HasValue<SharedInventorySlot>("shared_inventory_slot")
				.HasValue<MistChampionSkinUnlocker>("mist_champion_skin_unlocker")
				.HasValue<JadeBotSkinUnlocker>("jade_bot_skin_unlocker");
			itemBuilder.HasIndex((Item item) => item.Name);
			itemBuilder.HasIndex((Item item) => item.ChatLink);
			ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> attributesConverter = new ValueConverter<IDictionary<Extensible<AttributeName>, int>, string>((IDictionary<Extensible<AttributeName>, int> attr) => Serialize(attr.ToDictionary((KeyValuePair<Extensible<AttributeName>, int> pair) => ((object)pair.Key).ToString(), (KeyValuePair<Extensible<AttributeName>, int> pair) => pair.Value)), (string json) => Deserialize<Dictionary<string, int>>(json).ToDictionary((KeyValuePair<string, int> pair) => new Extensible<AttributeName>(pair.Key), (KeyValuePair<string, int> pair) => pair.Value));
			ValueComparer<IDictionary<Extensible<AttributeName>, int>> attributesComparer = new DictionaryComparer<Extensible<AttributeName>, int>();
			ValueComparer<Buff> buffComparer = new ValueObjectComparer<Buff>((Buff buff) => new Buff
			{
				Description = buff.Description,
				SkillId = buff.SkillId
			});
			ValueComparer<IReadOnlyList<InfusionSlot>> infusionSlotsComparer = new ListComparer<InfusionSlot>();
			ValueComparer<IReadOnlyCollection<InfusionSlotUpgradeSource>> upgradeSourceComparer = new CollectionComparer<InfusionSlotUpgradeSource>();
			ValueComparer<IReadOnlyCollection<InfusionSlotUpgradePath>> upgradePathComparer = new CollectionComparer<InfusionSlotUpgradePath>();
			itemBuilder.Property((Item item) => item.Id).ValueGeneratedNever();
			itemBuilder.Property((Item item) => item.Rarity).HasConversion(new ExtensibleEnumConverter<Rarity>());
			itemBuilder.Property((Item item) => item.VendorValue).HasConversion(new CoinConverter());
			itemBuilder.Property((Item item) => item.GameTypes).HasJsonValueConversion().Metadata.SetValueComparer(new CollectionComparer<Extensible<GameType>>());
			itemBuilder.Property((Item item) => item.Flags).HasJsonValueConversion();
			itemBuilder.Property((Item item) => item.Restrictions).HasJsonValueConversion();
			EntityTypeBuilder<Armor> armorBuilder = modelBuilder.Entity<Armor>();
			armorBuilder.Property((Armor armor) => armor.WeightClass).HasConversion(new ExtensibleEnumConverter<WeightClass>());
			armorBuilder.Property((Armor armor) => armor.Defense).HasColumnName("Defense");
			armorBuilder.Property((Armor armor) => armor.DefaultSkinId).HasColumnName("DefaultSkinId");
			armorBuilder.Property((Armor armor) => armor.SuffixItemId).HasColumnName("SuffixItemId");
			armorBuilder.Property((Armor armor) => armor.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			armorBuilder.Property((Armor armor) => armor.Attributes).HasColumnName("Attributes").HasConversion(attributesConverter)
				.Metadata.SetValueComparer(attributesComparer);
			armorBuilder.Property((Armor armor) => armor.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			armorBuilder.Property((Armor armor) => armor.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<int>());
			armorBuilder.Property((Armor armor) => armor.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion()
				.Metadata.SetValueComparer(infusionSlotsComparer);
			armorBuilder.Property((Armor armor) => armor.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(buffComparer);
			EntityTypeBuilder<Trinket> trinketBuilder = modelBuilder.Entity<Trinket>();
			trinketBuilder.Property((Trinket trinket) => trinket.SuffixItemId).HasColumnName("SuffixItemId");
			trinketBuilder.Property((Trinket trinket) => trinket.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			trinketBuilder.Property((Trinket trinket) => trinket.Attributes).HasColumnName("Attributes").HasConversion(attributesConverter)
				.Metadata.SetValueComparer(attributesComparer);
			trinketBuilder.Property((Trinket trinket) => trinket.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			trinketBuilder.Property((Trinket trinket) => trinket.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<int>());
			trinketBuilder.Property((Trinket trinket) => trinket.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion()
				.Metadata.SetValueComparer(infusionSlotsComparer);
			trinketBuilder.Property((Trinket trinket) => trinket.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(buffComparer);
			EntityTypeBuilder<Ring> ringBuilder = modelBuilder.Entity<Ring>();
			ringBuilder.Property((Ring ring) => ring.UpgradesFrom).HasColumnName("UpgradesFrom").HasJsonValueConversion()
				.Metadata.SetValueComparer(upgradeSourceComparer);
			ringBuilder.Property((Ring ring) => ring.UpgradesInto).HasColumnName("UpgradesInto").HasJsonValueConversion()
				.Metadata.SetValueComparer(upgradePathComparer);
			EntityTypeBuilder<Weapon> weaponBuilder = modelBuilder.Entity<Weapon>();
			weaponBuilder.Property((Weapon weapon) => weapon.DamageType).HasConversion(new ExtensibleEnumConverter<DamageType>());
			weaponBuilder.Property((Weapon weapon) => weapon.Defense).HasColumnName("Defense");
			weaponBuilder.Property((Weapon weapon) => weapon.DefaultSkinId).HasColumnName("DefaultSkinId");
			weaponBuilder.Property((Weapon weapon) => weapon.SuffixItemId).HasColumnName("SuffixItemId");
			weaponBuilder.Property((Weapon weapon) => weapon.SecondarySuffixItemId).HasColumnName("SecondarySuffixItemId");
			weaponBuilder.Property((Weapon weapon) => weapon.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			weaponBuilder.Property((Weapon weapon) => weapon.Attributes).HasColumnName("Attributes").HasConversion(attributesConverter)
				.Metadata.SetValueComparer(attributesComparer);
			weaponBuilder.Property((Weapon weapon) => weapon.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			weaponBuilder.Property((Weapon weapon) => weapon.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<int>());
			weaponBuilder.Property((Weapon weapon) => weapon.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion()
				.Metadata.SetValueComparer(infusionSlotsComparer);
			weaponBuilder.Property((Weapon weapon) => weapon.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(buffComparer);
			EntityTypeBuilder<Backpack> backpackBuilder = modelBuilder.Entity<Backpack>();
			backpackBuilder.Property((Backpack backpack) => backpack.DefaultSkinId).HasColumnName("DefaultSkinId");
			backpackBuilder.Property((Backpack backpack) => backpack.SuffixItemId).HasColumnName("SuffixItemId");
			backpackBuilder.Property((Backpack backpack) => backpack.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			backpackBuilder.Property((Backpack backpack) => backpack.Attributes).HasColumnName("Attributes").HasConversion(attributesConverter)
				.Metadata.SetValueComparer(attributesComparer);
			backpackBuilder.Property((Backpack backpack) => backpack.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			backpackBuilder.Property((Backpack backpack) => backpack.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<int>());
			backpackBuilder.Property((Backpack backpack) => backpack.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion()
				.Metadata.SetValueComparer(infusionSlotsComparer);
			backpackBuilder.Property((Backpack backpack) => backpack.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(buffComparer);
			backpackBuilder.Property((Backpack backpack) => backpack.UpgradesFrom).HasColumnName("UpgradesFrom").HasJsonValueConversion()
				.Metadata.SetValueComparer(upgradeSourceComparer);
			backpackBuilder.Property((Backpack backpack) => backpack.UpgradesInto).HasColumnName("UpgradesInto").HasJsonValueConversion()
				.Metadata.SetValueComparer(upgradePathComparer);
			EntityTypeBuilder<UpgradeComponent> upgradeComponentBuilder = modelBuilder.Entity<UpgradeComponent>();
			upgradeComponentBuilder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			upgradeComponentBuilder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.Attributes).HasColumnName("Attributes").HasConversion(attributesConverter)
				.Metadata.SetValueComparer(attributesComparer);
			upgradeComponentBuilder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			upgradeComponentBuilder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.UpgradeComponentFlags).HasConversion(new JsonValueConverter<UpgradeComponentFlags>());
			upgradeComponentBuilder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.InfusionUpgradeFlags).HasConversion(new JsonValueConverter<InfusionSlotFlags>());
			upgradeComponentBuilder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(buffComparer);
			modelBuilder.Entity<Rune>().Property((Rune rune) => rune.Bonuses).HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<string>());
			modelBuilder.Entity<CraftingMaterial>().Property((CraftingMaterial craftingMaterial) => craftingMaterial.UpgradesInto).HasColumnName("UpgradesInto")
				.HasJsonValueConversion()
				.Metadata.SetValueComparer(upgradePathComparer);
			modelBuilder.Entity<RecipeSheet>().Property((RecipeSheet recipeSheet) => recipeSheet.ExtraRecipeIds).HasJsonValueConversion()
				.Metadata.SetValueComparer(new CollectionComparer<int>());
			modelBuilder.Entity<Transmutation>().Property((Transmutation transmutation) => transmutation.SkinIds).HasJsonValueConversion()
				.Metadata.SetValueComparer(new CollectionComparer<int>());
			modelBuilder.Entity<Food>().Property((Food food) => food.Effect).HasColumnName("Effect")
				.HasJsonValueConversion();
			EntityTypeBuilder<GenericConsumable> genericConsumableBuilder = modelBuilder.Entity<GenericConsumable>();
			genericConsumableBuilder.Property((GenericConsumable genericConsumable) => genericConsumable.Effect).HasColumnName("Effect").HasJsonValueConversion();
			genericConsumableBuilder.Property((GenericConsumable genericConsumable) => genericConsumable.GuildUpgradeId).HasColumnName("GuildUpgradeId");
			EntityTypeBuilder<Service> serviceBuilder = modelBuilder.Entity<Service>();
			serviceBuilder.Property((Service service) => service.Effect).HasColumnName("Effect").HasJsonValueConversion();
			serviceBuilder.Property((Service service) => service.GuildUpgradeId).HasColumnName("GuildUpgradeId");
			modelBuilder.Entity<Gizmo>().Property((Gizmo gizmo) => gizmo.GuildUpgradeId).HasColumnName("GuildUpgradeId");
			modelBuilder.Entity<Utility>().Property((Utility utility) => utility.Effect).HasColumnName("Effect")
				.HasJsonValueConversion();
		}
	}
}
