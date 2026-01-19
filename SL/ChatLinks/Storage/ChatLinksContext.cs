using System.Reflection;
using System.Text.Json;
using GuildWars2.Hero.Achievements;
using GuildWars2.Hero.Achievements.Categories;
using GuildWars2.Hero.Achievements.Groups;
using GuildWars2.Hero.Crafting.Recipes;
using GuildWars2.Hero.Equipment.Dyes;
using GuildWars2.Hero.Equipment.Finishers;
using GuildWars2.Hero.Equipment.Gliders;
using GuildWars2.Hero.Equipment.JadeBots;
using GuildWars2.Hero.Equipment.MailCarriers;
using GuildWars2.Hero.Equipment.Miniatures;
using GuildWars2.Hero.Equipment.Novelties;
using GuildWars2.Hero.Equipment.Outfits;
using GuildWars2.Hero.Equipment.Wardrobe;
using GuildWars2.Items;
using GuildWars2.Pvp.MistChampions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Models.Hero.Achievements;
using SL.ChatLinks.Storage.Models.Hero.Crafting;
using SL.ChatLinks.Storage.Models.Hero.Equipment.Dyes;
using SL.ChatLinks.Storage.Models.Hero.Equipment.Finishers;
using SL.ChatLinks.Storage.Models.Hero.Equipment.Gliders;
using SL.ChatLinks.Storage.Models.Hero.Equipment.JadeBots;
using SL.ChatLinks.Storage.Models.Hero.Equipment.MailCarriers;
using SL.ChatLinks.Storage.Models.Hero.Equipment.Miniatures;
using SL.ChatLinks.Storage.Models.Hero.Equipment.Novelties;
using SL.ChatLinks.Storage.Models.Hero.Equipment.Outfits;
using SL.ChatLinks.Storage.Models.Hero.Equipment.Wardrobe;
using SL.ChatLinks.Storage.Models.Items;
using SL.ChatLinks.Storage.Models.Pvp.MistChampions;
using SL.Common;

namespace SL.ChatLinks.Storage
{
	public class ChatLinksContext : DbContext
	{
		public static int SchemaVersion => 7;

		public DbSet<Item> Items => Set<Item>();

		public DbSet<EquipmentSkin> Skins => Set<EquipmentSkin>();

		public DbSet<Recipe> Recipes => Set<Recipe>();

		public DbSet<DyeColor> Colors => Set<DyeColor>();

		public DbSet<Finisher> Finishers => Set<Finisher>();

		public DbSet<GliderSkin> Gliders => Set<GliderSkin>();

		public DbSet<JadeBotSkin> JadeBots => Set<JadeBotSkin>();

		public DbSet<MailCarrier> MailCarrriers => Set<MailCarrier>();

		public DbSet<MistChampionSkin> MistChampions => Set<MistChampionSkin>();

		public DbSet<Novelty> Novelties => Set<Novelty>();

		public DbSet<Miniature> Miniatures => Set<Miniature>();

		public DbSet<Outfit> Outfits => Set<Outfit>();

		public DbSet<Achievement> Achievements => Set<Achievement>();

		public DbSet<AchievementCategory> AchievementCategories => Set<AchievementCategory>();

		public DbSet<AchievementGroup> AchievementGroups => Set<AchievementGroup>();

		public ChatLinksContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			ThrowHelper.ThrowIfNull(optionsBuilder, "optionsBuilder");
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite("Data Source=data.db");
			}
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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
			ThrowHelper.ThrowIfNull(modelBuilder, "modelBuilder");
			modelBuilder.ApplyConfiguration(new ItemEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ArmorEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new TrinketEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new RingEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new WeaponEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new WeaponEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new BackItemEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new UpgradeComponentEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new RuneEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new CraftingMaterialEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new RecipeSheetEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new TransmutationEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new FoodEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new GenericConsumableEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ServiceEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new GizmoEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new UtilityEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new EquipmentSkinEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ArmorSkinEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new WeaponSkinEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new DyeColorEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new RecipeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new GuildConsumableRecipeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new GuildDecorationRecipeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new GuildWvwUpgradeRecipeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new FinisherEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new GliderSkinEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new JadeBotSkinEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new MailCarrierEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new MiniatureEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new MistChampionSkinEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new NoveltyEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new OutfitEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new AchievementEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new AchievementCategoryEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new AchievementGroupEntityTypeConfiguration());
			MethodInfo levenshteinMethod = typeof(Levenshtein).GetMethod("LevenshteinDistance");
			modelBuilder.HasDbFunction(levenshteinMethod, delegate(DbFunctionBuilder b)
			{
				b.HasName("LevenshteinDistance");
				b.HasParameter("a");
				b.HasParameter("b");
			});
		}
	}
}
