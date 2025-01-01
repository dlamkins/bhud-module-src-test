using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using SL.ChatLinks.Storage;

namespace SL.ChatLinks.Migrations
{
	[DbContext(typeof(ChatLinksContext))]
	[Migration("20241231023244_ItemsIndex")]
	public class ItemsIndex : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex("IX_Items_ChatLink", "Items", "ChatLink");
			migrationBuilder.CreateIndex("IX_Items_Name", "Items", "Name");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex("IX_Items_ChatLink", "Items");
			migrationBuilder.DropIndex("IX_Items_Name", "Items");
		}

		protected override void BuildTargetModel(ModelBuilder modelBuilder)
		{
			modelBuilder.HasAnnotation("ProductVersion", "3.1.32");
			modelBuilder.Entity("GuildWars2.Items.Item", delegate(EntityTypeBuilder b)
			{
				b.Property<int>("Id").HasColumnType("INTEGER");
				b.Property<string>("ChatLink").IsRequired().HasColumnType("TEXT");
				b.Property<string>("Description").IsRequired().HasColumnType("TEXT");
				b.Property<string>("Flags").IsRequired().HasColumnType("TEXT");
				b.Property<string>("GameTypes").IsRequired().HasColumnType("TEXT");
				b.Property<string>("IconHref").HasColumnType("TEXT");
				b.Property<int>("Level").HasColumnType("INTEGER");
				b.Property<string>("Name").IsRequired().HasColumnType("TEXT");
				b.Property<string>("Rarity").IsRequired().HasColumnType("TEXT");
				b.Property<string>("Restrictions").IsRequired().HasColumnType("TEXT");
				b.Property<string>("Type").IsRequired().HasColumnType("TEXT");
				b.Property<int>("VendorValue").HasColumnType("INTEGER");
				b.HasKey("Id");
				b.HasIndex("ChatLink");
				b.HasIndex("Name");
				b.ToTable("Items");
				b.HasDiscriminator<string>("Type").HasValue("item");
			});
			modelBuilder.Entity("GuildWars2.Items.Armor", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<double>("AttributeAdjustment").HasColumnName("AttributeAdjustment").HasColumnType("REAL");
				b.Property<int?>("AttributeCombinationId").HasColumnName("AttributeCombinationId").HasColumnType("INTEGER");
				b.Property<string>("Attributes").IsRequired().HasColumnName("Attributes")
					.HasColumnType("TEXT");
				b.Property<string>("Buff").HasColumnName("Buff").HasColumnType("TEXT");
				b.Property<int>("DefaultSkinId").HasColumnName("DefaultSkinId").HasColumnType("INTEGER");
				b.Property<int>("Defense").HasColumnName("Defense").HasColumnType("INTEGER");
				b.Property<string>("InfusionSlots").IsRequired().HasColumnName("InfusionSlots")
					.HasColumnType("TEXT");
				b.Property<string>("StatChoices").IsRequired().HasColumnName("StatChoices")
					.HasColumnType("TEXT");
				b.Property<int?>("SuffixItemId").HasColumnName("SuffixItemId").HasColumnType("INTEGER");
				b.Property<string>("WeightClass").IsRequired().HasColumnType("TEXT");
				b.HasDiscriminator().HasValue("armor");
			});
			modelBuilder.Entity("GuildWars2.Items.Backpack", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<double>("AttributeAdjustment").HasColumnName("AttributeAdjustment").HasColumnType("REAL");
				b.Property<int?>("AttributeCombinationId").HasColumnName("AttributeCombinationId").HasColumnType("INTEGER");
				b.Property<string>("Attributes").IsRequired().HasColumnName("Attributes")
					.HasColumnType("TEXT");
				b.Property<string>("Buff").HasColumnName("Buff").HasColumnType("TEXT");
				b.Property<int>("DefaultSkinId").HasColumnName("DefaultSkinId").HasColumnType("INTEGER");
				b.Property<string>("InfusionSlots").IsRequired().HasColumnName("InfusionSlots")
					.HasColumnType("TEXT");
				b.Property<string>("StatChoices").IsRequired().HasColumnName("StatChoices")
					.HasColumnType("TEXT");
				b.Property<int?>("SuffixItemId").HasColumnName("SuffixItemId").HasColumnType("INTEGER");
				b.Property<string>("UpgradesFrom").IsRequired().HasColumnName("UpgradesFrom")
					.HasColumnType("TEXT");
				b.Property<string>("UpgradesInto").IsRequired().HasColumnName("UpgradesInto")
					.HasColumnType("TEXT");
				b.HasDiscriminator().HasValue("back");
			});
			modelBuilder.Entity("GuildWars2.Items.Bag", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<bool>("NoSellOrSort").HasColumnType("INTEGER");
				b.Property<int>("Size").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("bag");
			});
			modelBuilder.Entity("GuildWars2.Items.Consumable", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.HasDiscriminator().HasValue("consumable");
			});
			modelBuilder.Entity("GuildWars2.Items.Container", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.HasDiscriminator().HasValue("container");
			});
			modelBuilder.Entity("GuildWars2.Items.CraftingMaterial", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<string>("UpgradesInto").IsRequired().HasColumnName("UpgradesInto")
					.HasColumnType("TEXT");
				b.HasDiscriminator().HasValue("crafting_material");
			});
			modelBuilder.Entity("GuildWars2.Items.GatheringTool", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.HasDiscriminator().HasValue("gathering_tool");
			});
			modelBuilder.Entity("GuildWars2.Items.Gizmo", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<int?>("GuildUpgradeId").HasColumnName("GuildUpgradeId").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("gizmo");
			});
			modelBuilder.Entity("GuildWars2.Items.JadeTechModule", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.HasDiscriminator().HasValue("jade_tech_module");
			});
			modelBuilder.Entity("GuildWars2.Items.Miniature", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<int>("MiniatureId").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("miniature");
			});
			modelBuilder.Entity("GuildWars2.Items.PowerCore", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.HasDiscriminator().HasValue("power_core");
			});
			modelBuilder.Entity("GuildWars2.Items.Relic", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.HasDiscriminator().HasValue("relic");
			});
			modelBuilder.Entity("GuildWars2.Items.SalvageTool", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<int>("Charges").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("salvage_tool");
			});
			modelBuilder.Entity("GuildWars2.Items.Trinket", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<double>("AttributeAdjustment").HasColumnName("AttributeAdjustment").HasColumnType("REAL");
				b.Property<int?>("AttributeCombinationId").HasColumnName("AttributeCombinationId").HasColumnType("INTEGER");
				b.Property<string>("Attributes").IsRequired().HasColumnName("Attributes")
					.HasColumnType("TEXT");
				b.Property<string>("Buff").HasColumnName("Buff").HasColumnType("TEXT");
				b.Property<string>("InfusionSlots").IsRequired().HasColumnName("InfusionSlots")
					.HasColumnType("TEXT");
				b.Property<string>("StatChoices").IsRequired().HasColumnName("StatChoices")
					.HasColumnType("TEXT");
				b.Property<int?>("SuffixItemId").HasColumnName("SuffixItemId").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("trinket");
			});
			modelBuilder.Entity("GuildWars2.Items.Trophy", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.HasDiscriminator().HasValue("trophy");
			});
			modelBuilder.Entity("GuildWars2.Items.UpgradeComponent", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<double>("AttributeAdjustment").HasColumnName("AttributeAdjustment").HasColumnType("REAL");
				b.Property<int?>("AttributeCombinationId").HasColumnName("AttributeCombinationId").HasColumnType("INTEGER");
				b.Property<string>("Attributes").IsRequired().HasColumnName("Attributes")
					.HasColumnType("TEXT");
				b.Property<string>("Buff").HasColumnName("Buff").HasColumnType("TEXT");
				b.Property<string>("InfusionUpgradeFlags").IsRequired().HasColumnType("TEXT");
				b.Property<string>("SuffixName").IsRequired().HasColumnType("TEXT");
				b.Property<string>("UpgradeComponentFlags").IsRequired().HasColumnType("TEXT");
				b.HasDiscriminator().HasValue("upgrade_component");
			});
			modelBuilder.Entity("GuildWars2.Items.Weapon", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Item");
				b.Property<double>("AttributeAdjustment").HasColumnName("AttributeAdjustment").HasColumnType("REAL");
				b.Property<int?>("AttributeCombinationId").HasColumnName("AttributeCombinationId").HasColumnType("INTEGER");
				b.Property<string>("Attributes").IsRequired().HasColumnName("Attributes")
					.HasColumnType("TEXT");
				b.Property<string>("Buff").HasColumnName("Buff").HasColumnType("TEXT");
				b.Property<string>("DamageType").IsRequired().HasColumnType("TEXT");
				b.Property<int>("DefaultSkinId").HasColumnName("DefaultSkinId").HasColumnType("INTEGER");
				b.Property<int>("Defense").HasColumnName("Defense").HasColumnType("INTEGER");
				b.Property<string>("InfusionSlots").IsRequired().HasColumnName("InfusionSlots")
					.HasColumnType("TEXT");
				b.Property<int>("MaxPower").HasColumnType("INTEGER");
				b.Property<int>("MinPower").HasColumnType("INTEGER");
				b.Property<int?>("SecondarySuffixItemId").HasColumnName("SecondarySuffixItemId").HasColumnType("INTEGER");
				b.Property<string>("StatChoices").IsRequired().HasColumnName("StatChoices")
					.HasColumnType("TEXT");
				b.Property<int?>("SuffixItemId").HasColumnName("SuffixItemId").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("weapon");
			});
			modelBuilder.Entity("GuildWars2.Items.Boots", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Armor");
				b.HasDiscriminator().HasValue("boots");
			});
			modelBuilder.Entity("GuildWars2.Items.Coat", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Armor");
				b.HasDiscriminator().HasValue("coat");
			});
			modelBuilder.Entity("GuildWars2.Items.Gloves", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Armor");
				b.HasDiscriminator().HasValue("gloves");
			});
			modelBuilder.Entity("GuildWars2.Items.Helm", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Armor");
				b.HasDiscriminator().HasValue("helm");
			});
			modelBuilder.Entity("GuildWars2.Items.HelmAquatic", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Armor");
				b.HasDiscriminator().HasValue("helm_aquatic");
			});
			modelBuilder.Entity("GuildWars2.Items.Leggings", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Armor");
				b.HasDiscriminator().HasValue("leggings");
			});
			modelBuilder.Entity("GuildWars2.Items.Shoulders", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Armor");
				b.HasDiscriminator().HasValue("shoulders");
			});
			modelBuilder.Entity("GuildWars2.Items.AppearanceChanger", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("appearance_changer");
			});
			modelBuilder.Entity("GuildWars2.Items.Booze", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("booze");
			});
			modelBuilder.Entity("GuildWars2.Items.ContractNpc", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("contract_npc");
			});
			modelBuilder.Entity("GuildWars2.Items.Currency", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("currency");
			});
			modelBuilder.Entity("GuildWars2.Items.Food", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.Property<string>("Effect").HasColumnName("Effect").HasColumnType("TEXT");
				b.HasDiscriminator().HasValue("food");
			});
			modelBuilder.Entity("GuildWars2.Items.GenericConsumable", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.Property<string>("Effect").HasColumnName("Effect").HasColumnType("TEXT");
				b.Property<int?>("GuildUpgradeId").HasColumnName("GuildUpgradeId").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("generic_consumable");
			});
			modelBuilder.Entity("GuildWars2.Items.HalloweenConsumable", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("halloween_consumable");
			});
			modelBuilder.Entity("GuildWars2.Items.MountLicense", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("mount_license");
			});
			modelBuilder.Entity("GuildWars2.Items.RandomUnlocker", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("random_unlocker");
			});
			modelBuilder.Entity("GuildWars2.Items.Service", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.Property<string>("Effect").HasColumnName("Effect").HasColumnType("TEXT");
				b.Property<int?>("GuildUpgradeId").HasColumnName("GuildUpgradeId").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("service");
			});
			modelBuilder.Entity("GuildWars2.Items.TeleportToFriend", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("teleport_to_friend");
			});
			modelBuilder.Entity("GuildWars2.Items.Transmutation", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.Property<string>("SkinIds").IsRequired().HasColumnType("TEXT");
				b.HasDiscriminator().HasValue("transmutation");
			});
			modelBuilder.Entity("GuildWars2.Items.Unlocker", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("unlocker");
			});
			modelBuilder.Entity("GuildWars2.Items.UpgradeExtractor", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.HasDiscriminator().HasValue("upgrade_extractor");
			});
			modelBuilder.Entity("GuildWars2.Items.Utility", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Consumable");
				b.Property<string>("Effect").HasColumnName("Effect").HasColumnType("TEXT");
				b.HasDiscriminator().HasValue("utility");
			});
			modelBuilder.Entity("GuildWars2.Items.BlackLionChest", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Container");
				b.HasDiscriminator().HasValue("black_lion_chest");
			});
			modelBuilder.Entity("GuildWars2.Items.GiftBox", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Container");
				b.HasDiscriminator().HasValue("gift_box");
			});
			modelBuilder.Entity("GuildWars2.Items.ImmediateContainer", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Container");
				b.HasDiscriminator().HasValue("immediate_container");
			});
			modelBuilder.Entity("GuildWars2.Items.Bait", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.GatheringTool");
				b.HasDiscriminator().HasValue("bait");
			});
			modelBuilder.Entity("GuildWars2.Items.HarvestingSickle", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.GatheringTool");
				b.HasDiscriminator().HasValue("harvesting_sickle");
			});
			modelBuilder.Entity("GuildWars2.Items.LoggingAxe", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.GatheringTool");
				b.HasDiscriminator().HasValue("logging_axe");
			});
			modelBuilder.Entity("GuildWars2.Items.Lure", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.GatheringTool");
				b.HasDiscriminator().HasValue("lure");
			});
			modelBuilder.Entity("GuildWars2.Items.MiningPick", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.GatheringTool");
				b.HasDiscriminator().HasValue("mining_pick");
			});
			modelBuilder.Entity("GuildWars2.Items.BlackLionChestKey", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Gizmo");
				b.HasDiscriminator().HasValue("black_lion_chest_key");
			});
			modelBuilder.Entity("GuildWars2.Items.RentableContractNpc", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Gizmo");
				b.HasDiscriminator().HasValue("rentable_contract_npc");
			});
			modelBuilder.Entity("GuildWars2.Items.UnlimitedConsumable", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Gizmo");
				b.HasDiscriminator().HasValue("unlimited_consumable");
			});
			modelBuilder.Entity("GuildWars2.Items.Accessory", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Trinket");
				b.HasDiscriminator().HasValue("accessory");
			});
			modelBuilder.Entity("GuildWars2.Items.Amulet", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Trinket");
				b.HasDiscriminator().HasValue("amulet");
			});
			modelBuilder.Entity("GuildWars2.Items.Ring", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Trinket");
				b.Property<string>("UpgradesFrom").IsRequired().HasColumnName("UpgradesFrom")
					.HasColumnType("TEXT");
				b.Property<string>("UpgradesInto").IsRequired().HasColumnName("UpgradesInto")
					.HasColumnType("TEXT");
				b.HasDiscriminator().HasValue("ring");
			});
			modelBuilder.Entity("GuildWars2.Items.Gem", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.UpgradeComponent");
				b.HasDiscriminator().HasValue("gem");
			});
			modelBuilder.Entity("GuildWars2.Items.Rune", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.UpgradeComponent");
				b.Property<string>("Bonuses").HasColumnType("TEXT");
				b.HasDiscriminator().HasValue("rune");
			});
			modelBuilder.Entity("GuildWars2.Items.Sigil", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.UpgradeComponent");
				b.HasDiscriminator().HasValue("sigil");
			});
			modelBuilder.Entity("GuildWars2.Items.Axe", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("axe");
			});
			modelBuilder.Entity("GuildWars2.Items.Dagger", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("dagger");
			});
			modelBuilder.Entity("GuildWars2.Items.Focus", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("focus");
			});
			modelBuilder.Entity("GuildWars2.Items.Greatsword", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("greatsword");
			});
			modelBuilder.Entity("GuildWars2.Items.Hammer", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("hammer");
			});
			modelBuilder.Entity("GuildWars2.Items.HarpoonGun", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("harpoon_gun");
			});
			modelBuilder.Entity("GuildWars2.Items.LargeBundle", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("large_bundle");
			});
			modelBuilder.Entity("GuildWars2.Items.Longbow", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("longbow");
			});
			modelBuilder.Entity("GuildWars2.Items.Mace", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("mace");
			});
			modelBuilder.Entity("GuildWars2.Items.Pistol", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("pistol");
			});
			modelBuilder.Entity("GuildWars2.Items.Rifle", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("rifle");
			});
			modelBuilder.Entity("GuildWars2.Items.Scepter", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("scepter");
			});
			modelBuilder.Entity("GuildWars2.Items.Shield", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("shield");
			});
			modelBuilder.Entity("GuildWars2.Items.Shortbow", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("shortbow");
			});
			modelBuilder.Entity("GuildWars2.Items.SmallBundle", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("small_bundle");
			});
			modelBuilder.Entity("GuildWars2.Items.Spear", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("spear");
			});
			modelBuilder.Entity("GuildWars2.Items.Staff", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("staff");
			});
			modelBuilder.Entity("GuildWars2.Items.Sword", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("sword");
			});
			modelBuilder.Entity("GuildWars2.Items.Torch", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("torch");
			});
			modelBuilder.Entity("GuildWars2.Items.Toy", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("toy");
			});
			modelBuilder.Entity("GuildWars2.Items.ToyTwoHanded", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("toy_two_handed");
			});
			modelBuilder.Entity("GuildWars2.Items.Trident", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("trident");
			});
			modelBuilder.Entity("GuildWars2.Items.Warhorn", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Weapon");
				b.HasDiscriminator().HasValue("warhorn");
			});
			modelBuilder.Entity("GuildWars2.Items.BagSlotExpansion", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("bag_slot_expansion");
			});
			modelBuilder.Entity("GuildWars2.Items.BankTabExpansion", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("bank_tab_expansion");
			});
			modelBuilder.Entity("GuildWars2.Items.BuildStorageExpansion", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("build_storage_expansion");
			});
			modelBuilder.Entity("GuildWars2.Items.BuildTemplateExpansion", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("build_template_expansion");
			});
			modelBuilder.Entity("GuildWars2.Items.ContentUnlocker", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("content_unlocker");
			});
			modelBuilder.Entity("GuildWars2.Items.Dye", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.Property<int>("ColorId").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("dye");
			});
			modelBuilder.Entity("GuildWars2.Items.EquipmentTemplateExpansion", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("equipment_template_expansion");
			});
			modelBuilder.Entity("GuildWars2.Items.GliderSkinUnlocker", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("glider_skin_unlocker");
			});
			modelBuilder.Entity("GuildWars2.Items.JadeBotSkinUnlocker", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("jade_bot_skin_unlocker");
			});
			modelBuilder.Entity("GuildWars2.Items.MiniatureUnlocker", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("miniature_unlocker");
			});
			modelBuilder.Entity("GuildWars2.Items.MistChampionSkinUnlocker", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("mist_champion_skin_unlocker");
			});
			modelBuilder.Entity("GuildWars2.Items.MountSkinUnlocker", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("mount_skin_unlocker");
			});
			modelBuilder.Entity("GuildWars2.Items.OutfitUnlocker", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("outfit_unlocker");
			});
			modelBuilder.Entity("GuildWars2.Items.RecipeSheet", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.Property<string>("ExtraRecipeIds").IsRequired().HasColumnType("TEXT");
				b.Property<int>("RecipeId").HasColumnType("INTEGER");
				b.HasDiscriminator().HasValue("recipe_sheet");
			});
			modelBuilder.Entity("GuildWars2.Items.SharedInventorySlot", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("shared_inventory_slot");
			});
			modelBuilder.Entity("GuildWars2.Items.StorageExpander", delegate(EntityTypeBuilder b)
			{
				b.HasBaseType("GuildWars2.Items.Unlocker");
				b.HasDiscriminator().HasValue("storage_expander");
			});
		}
	}
}
